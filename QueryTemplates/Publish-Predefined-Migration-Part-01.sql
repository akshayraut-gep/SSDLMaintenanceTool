DECLARE @TotalJobs BIGINT
DECLARE @Iterator BIGINT = 1

DECLARE @JobsWithPredefinedQueries AS TABLE
(
    JobId INT
)

DECLARE @JobsDetails AS TABLE
(
    JobId INT,
    JobName VARCHAR(100),
    ParentJobId INT
)

DECLARE @FinalJobs AS TABLE
(
    JobId INT,
    JobName VARCHAR(100),
    ParentJobId INT,
    ParentJobName VARCHAR(100),
    RowNumber BIGINT
)

INSERT INTO @JobsWithPredefinedQueries
SELECT DISTINCT JobId
FROM SSDL.JOB_DETAILS
WHERE SettingName = 'PreDefinedSteps'

INSERT INTO @JobsDetails
SELECT JPQ.JobId, a1.JOB_NAME AS JobName, a1.PARENT_JOB_ID AS ParentJobId
FROM @JobsWithPredefinedQueries JPQ
INNER JOIN SSDL.SPEND_DL_SA_ACIVITYWORKMASTER a1 ON JPQ.JobId = a1.JOB_ID AND ISNULL(a1.IsDeleted, 0) = 0
INNER JOIN SSDL.SPEND_DL_SA_ACTIVITYWORKTRANSACTIONS a2 ON a1.JOB_ID = a2.JOB_ID AND a2.ACTIVITY_ID = 6600
    AND a1.JOB_STATUS IN ('M', 'N', 'I', 'SM', 'C', 'SL', 'E') AND a2.ACTIVITY_STATUS NOT IN ('I','D')
ORDER BY JPQ.JobId

INSERT INTO @FinalJobs
SELECT JD.*, parent.JOB_NAME AS ParentJobName, ROW_NUMBER() OVER(ORDER BY JobId) AS RowNumber
FROM @JobsDetails JD
LEFT JOIN SSDL.SPEND_DL_SA_ACIVITYWORKMASTER parent ON JD.ParentJobId IS NOT NULL AND JD.ParentJobId = parent.JOB_ID
ORDER BY JobId

-- select * from @FinalJobs

select @TotalJobs = count(1) from @FinalJobs
DECLARE @PreDefinedQueries AS TABLE
(
    JobId INT,
    JobName VARCHAR(100),
    ParentJobId INT,
    ParentJobName VARCHAR(100),
    Sequence BIGINT,
    QueryName NVARCHAR(255),
    Query VARCHAR(MAX),
    SettingValue NVARCHAR(MAX)
)

WHILE @Iterator < = @TotalJobs
BEGIN
    DECLARE @JobId BIGINT, @JobName VARCHAR(100), @ParentJobId BIGINT, @ParentJobName VARCHAR(100), @SettingValue NVARCHAR(MAX);

    select @JobId = b.JobID, @JobName = b.JobName, @ParentJobId = b.ParentJobId, @ParentJobName = b.ParentJobName, @SettingValue = SettingValue
    from SSDL.JOB_DETAILS a
    join @FinalJobs b on a.JobID = b.JobID
    where SettingName = 'PreDefinedSteps' and b.RowNumber = @Iterator

    INSERT INTO @PreDefinedQueries
    SELECT @JobId AS JobId, @JobName AS JobName, @ParentJobId AS ParentJobId, @ParentJobName AS ParentJobName,
        a.[key] as [Sequence], QueryName, [Query],
        --    @SettingValue
        --    JSON_VALUE(a.[value], '$.queryName') as QueryName,
        --    JSON_VALUE(a.[value], '$.jsonFormat') as Query,
        --    jsonfomat,
        '[' + a.[value] + ']' as SettingValue
    FROM OPENJSON(@SettingValue) as a
    CROSS APPLY OPENJSON(a.[value])
    WITH (
        QueryName nvarchar(max) '$.queryName',
        [Query] nvarchar(max) '$.jsonFormat'
    )
    -----
    SET @Iterator = @Iterator + 1;
END

SELECT * FROM @PreDefinedQueries