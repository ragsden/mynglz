/*
Deployment script for mynglzdb
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "mynglzdb"
:setvar DefaultDataPath ""
:setvar DefaultLogPath ""

GO
USE [master]

GO
:on error exit
GO
IF (DB_ID(N'$(DatabaseName)') IS NOT NULL
    AND DATABASEPROPERTYEX(N'$(DatabaseName)','Status') <> N'ONLINE')
BEGIN
    RAISERROR(N'The state of the target database, %s, is not set to ONLINE. To deploy to this database, its state must be set to ONLINE.', 16, 127,N'$(DatabaseName)') WITH NOWAIT
    RETURN
END

GO
IF (DB_ID(N'$(DatabaseName)') IS NOT NULL) 
BEGIN
    ALTER DATABASE [$(DatabaseName)]
    SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [$(DatabaseName)];
END

GO
PRINT N'Creating $(DatabaseName)...'
GO
CREATE DATABASE [$(DatabaseName)] COLLATE SQL_Latin1_General_CP1_CI_AS
GO
EXECUTE sp_dbcmptlevel [$(DatabaseName)], 100;


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET ANSI_NULLS ON,
                ANSI_PADDING ON,
                ANSI_WARNINGS ON,
                ARITHABORT ON,
                CONCAT_NULL_YIELDS_NULL ON,
                NUMERIC_ROUNDABORT OFF,
                QUOTED_IDENTIFIER ON,
                ANSI_NULL_DEFAULT ON,
                CURSOR_DEFAULT LOCAL,
                RECOVERY FULL,
                CURSOR_CLOSE_ON_COMMIT OFF,
                AUTO_CREATE_STATISTICS ON,
                AUTO_SHRINK OFF,
                AUTO_UPDATE_STATISTICS ON,
                RECURSIVE_TRIGGERS OFF 
            WITH ROLLBACK IMMEDIATE;
        ALTER DATABASE [$(DatabaseName)]
            SET AUTO_CLOSE OFF 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET ALLOW_SNAPSHOT_ISOLATION OFF;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET READ_COMMITTED_SNAPSHOT OFF;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET AUTO_UPDATE_STATISTICS_ASYNC OFF,
                PAGE_VERIFY NONE,
                DATE_CORRELATION_OPTIMIZATION OFF,
                DISABLE_BROKER,
                PARAMETERIZATION SIMPLE,
                SUPPLEMENTAL_LOGGING OFF 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF IS_SRVROLEMEMBER(N'sysadmin') = 1
    BEGIN
        IF EXISTS (SELECT 1
                   FROM   [master].[dbo].[sysdatabases]
                   WHERE  [name] = N'$(DatabaseName)')
            BEGIN
                EXECUTE sp_executesql N'ALTER DATABASE [$(DatabaseName)]
    SET TRUSTWORTHY OFF,
        DB_CHAINING OFF 
    WITH ROLLBACK IMMEDIATE';
            END
    END
ELSE
    BEGIN
        PRINT N'The database settings cannot be modified. You must be a SysAdmin to apply these settings.';
    END


GO
IF IS_SRVROLEMEMBER(N'sysadmin') = 1
    BEGIN
        IF EXISTS (SELECT 1
                   FROM   [master].[dbo].[sysdatabases]
                   WHERE  [name] = N'$(DatabaseName)')
            BEGIN
                EXECUTE sp_executesql N'ALTER DATABASE [$(DatabaseName)]
    SET HONOR_BROKER_PRIORITY OFF 
    WITH ROLLBACK IMMEDIATE';
            END
    END
ELSE
    BEGIN
        PRINT N'The database settings cannot be modified. You must be a SysAdmin to apply these settings.';
    END


GO
USE [$(DatabaseName)]

GO
IF fulltextserviceproperty(N'IsFulltextInstalled') = 1
    EXECUTE sp_fulltext_database 'enable';


GO
/*
 Pre-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.	
 Use SQLCMD syntax to include a file in the pre-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the pre-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

GO
PRINT N'Creating [dbo].[Business]...';


GO
CREATE TABLE [dbo].[Business] (
    [BusinessId]         INT             IDENTITY (1, 1) NOT NULL,
    [ExternalId]         VARCHAR (30)    NOT NULL,
    [GenderMCheckIns]    INT             NOT NULL,
    [GenderFCheckIns]    INT             NOT NULL,
    [SourceId]           INT             NULL,
    [ExternalIdCheckSum] VARBINARY (100) NOT NULL,
    [LastDateCheckin]    DATETIME        NULL,
    [UpdatedDtTm]        DATETIME        NULL,
    [UpdatedBy]          VARCHAR (80)    NULL,
    CONSTRAINT [XPKBusiness] PRIMARY KEY CLUSTERED ([BusinessId] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF)
);


GO
PRINT N'Creating [dbo].[Business].[UNCIExternalIdCheckSum]...';


GO
CREATE UNIQUE NONCLUSTERED INDEX [UNCIExternalIdCheckSum]
    ON [dbo].[Business]([ExternalIdCheckSum] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0);


GO
PRINT N'Creating [dbo].[UserChat]...';


GO
CREATE TABLE [dbo].[UserChat] (
    [ChatId]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [HigherUserID]    INT            NOT NULL,
    [LowerUserID]     INT            NOT NULL,
    [IsHigherMessage] BIT            NOT NULL,
    [UserMessage]     NVARCHAR (120) NOT NULL,
    [CreatedDtTm]     DATETIME       NOT NULL,
    CONSTRAINT [XPKUserChat] PRIMARY KEY CLUSTERED ([ChatId] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF)
);


GO
PRINT N'Creating [dbo].[UserChat].[UNCIHighLowId]...';


GO
CREATE NONCLUSTERED INDEX [UNCIHighLowId]
    ON [dbo].[UserChat]([HigherUserID] ASC, [LowerUserID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0);


GO
PRINT N'Creating [dbo].[UserChatHistory]...';


GO
CREATE TABLE [dbo].[UserChatHistory] (
    [ChatId]          BIGINT         NOT NULL,
    [HigherUserID]    INT            NOT NULL,
    [LowerUserID]     INT            NOT NULL,
    [IsHigherMessage] BIT            NOT NULL,
    [UserMessage]     NVARCHAR (120) NOT NULL,
    [CreatedDtTm]     DATETIME       NOT NULL
);


GO
PRINT N'Creating [dbo].[UserCheckIn]...';


GO
CREATE TABLE [dbo].[UserCheckIn] (
    [UserID]       INT      NOT NULL,
    [StartTime]    DATETIME NULL,
    [CheckinCount] INT      NULL,
    [UpdatedDtTm]  DATETIME NOT NULL,
    [BusinessId]   INT      NULL,
    CONSTRAINT [XPKBusinessCheckIn] PRIMARY KEY CLUSTERED ([UserID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF)
);


GO
PRINT N'Creating [dbo].[UserCheckIn].[NCIBusinessID]...';


GO
CREATE NONCLUSTERED INDEX [NCIBusinessID]
    ON [dbo].[UserCheckIn]([BusinessId] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0);


GO
PRINT N'Creating [dbo].[UserCheckIn].[NCIUpdatedDtTm]...';


GO
CREATE NONCLUSTERED INDEX [NCIUpdatedDtTm]
    ON [dbo].[UserCheckIn]([UpdatedDtTm] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0);


GO
PRINT N'Creating [dbo].[UserCheckIn].[NCIUserID]...';


GO
CREATE NONCLUSTERED INDEX [NCIUserID]
    ON [dbo].[UserCheckIn]([UserID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0);


GO
PRINT N'Creating [dbo].[UserCheckInHistory]...';


GO
CREATE TABLE [dbo].[UserCheckInHistory] (
    [UserID]       INT          NOT NULL,
    [StartTime]    DATETIME     NOT NULL,
    [EndTime]      DATETIME     NOT NULL,
    [CheckinCount] INT          NULL,
    [UpdatedDtTm]  DATETIME     NOT NULL,
    [UpdatedBy]    VARCHAR (20) NULL,
    [BusinessId]   INT          NOT NULL,
    [CheckinID]    INT          IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [XPKBusinessCheckInHistory] PRIMARY KEY CLUSTERED ([CheckinID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF)
);


GO
PRINT N'Creating [dbo].[UserImageHistory]...';


GO
CREATE TABLE [dbo].[UserImageHistory] (
    [UserID]          INT            NOT NULL,
    [PictureLocation] VARCHAR (8000) NULL,
    [CreatedDtTm]     DATETIME       NOT NULL
);


GO
PRINT N'Creating [dbo].[UserInfo]...';


GO
CREATE TABLE [dbo].[UserInfo] (
    [UserID]          INT            NOT NULL,
    [Handle]          NVARCHAR (60)  NULL,
    [Email]           NVARCHAR (140) NULL,
    [InstantTitle]    VARCHAR (140)  NULL,
    [PictureLocation] VARCHAR (8000) NULL,
    [OnlineStatusID]  SMALLINT       NOT NULL,
    [UpdatedDtTm]     DATETIME       NULL,
    [UpdatedBy]       VARCHAR (80)   NULL,
    [Gender]          SMALLINT       NULL,
    [Preference]      SMALLINT       NULL,
    [Age]             SMALLINT       NOT NULL,
    [CreatedDtTm]     DATETIME       NOT NULL,
    [CreatedBy]       VARCHAR (20)   NULL,
    CONSTRAINT [XPKUserInfo] PRIMARY KEY CLUSTERED ([UserID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF)
);


GO
PRINT N'Creating [dbo].[UserLogin]...';


GO
CREATE TABLE [dbo].[UserLogin] (
    [UserID]             INT             IDENTITY (1, 1) NOT NULL,
    [UserName]           NVARCHAR (10)   NULL,
    [UserPassword]       NVARCHAR (20)   NULL,
    [ExternalId]         VARCHAR (30)    NULL,
    [SourceID]           SMALLINT        NOT NULL,
    [ExternalIdCheckSum] VARBINARY (100) NOT NULL,
    [LastLoginDateTime]  DATETIME        NOT NULL,
    [CreatedDtTm]        DATETIME        NOT NULL,
    [CreatedBy]          VARCHAR (20)    NULL,
    [Disabled]           BIT             NOT NULL,
    CONSTRAINT [XPKUserLogin] PRIMARY KEY CLUSTERED ([UserID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF)
);


GO
PRINT N'Creating [dbo].[UserLogin].[UNCIExternalIdCheckSum]...';


GO
CREATE UNIQUE NONCLUSTERED INDEX [UNCIExternalIdCheckSum]
    ON [dbo].[UserLogin]([ExternalIdCheckSum] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0);


GO
PRINT N'Creating [dbo].[UserSelection]...';


GO
CREATE TABLE [dbo].[UserSelection] (
    [FromUserID]  INT      NOT NULL,
    [ToUserId]    INT      NOT NULL,
    [StatusId]    SMALLINT NOT NULL,
    [UpdatedDtTm] DATETIME NOT NULL,
    CONSTRAINT [XPKUserSelection] PRIMARY KEY NONCLUSTERED ([FromUserID] ASC, [ToUserId] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF)
);


GO
PRINT N'Creating [dbo].[UserSelection].[UNCIBusinessIdFromUserID]...';


GO
CREATE UNIQUE NONCLUSTERED INDEX [UNCIBusinessIdFromUserID]
    ON [dbo].[UserSelection]([FromUserID] ASC, [ToUserId] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0);


GO
PRINT N'Creating [dbo].[UserSelectionHistory]...';


GO
CREATE TABLE [dbo].[UserSelectionHistory] (
    [UserCheckInId] INT      NOT NULL,
    [FromUserID]    INT      NOT NULL,
    [ToUserId]      INT      NOT NULL,
    [StatusId]      SMALLINT NOT NULL,
    [UpdatedDtTm]   DATETIME NOT NULL
);


GO
PRINT N'Creating [dbo].[BusinessLight]...';


GO
CREATE TYPE [dbo].[BusinessLight] AS  TABLE (
    [ExternalId] VARCHAR (MAX) NOT NULL,
    [SourceId]   INT           NULL);


GO
PRINT N'Creating On column: GenderMCheckIns...';


GO
ALTER TABLE [dbo].[Business]
    ADD DEFAULT (0) FOR [GenderMCheckIns];


GO
PRINT N'Creating On column: GenderFCheckIns...';


GO
ALTER TABLE [dbo].[Business]
    ADD DEFAULT (0) FOR [GenderFCheckIns];


GO
PRINT N'Creating On column: UpdatedDtTm...';


GO
ALTER TABLE [dbo].[Business]
    ADD DEFAULT (GETUTCDATE()) FOR [UpdatedDtTm];


GO
PRINT N'Creating On column: IsHigherMessage...';


GO
ALTER TABLE [dbo].[UserChat]
    ADD DEFAULT (0) FOR [IsHigherMessage];


GO
PRINT N'Creating On column: CreatedDtTm...';


GO
ALTER TABLE [dbo].[UserChat]
    ADD DEFAULT (GETUTCDATE()) FOR [CreatedDtTm];


GO
PRINT N'Creating On column: IsHigherMessage...';


GO
ALTER TABLE [dbo].[UserChatHistory]
    ADD DEFAULT (0) FOR [IsHigherMessage];


GO
PRINT N'Creating On column: CreatedDtTm...';


GO
ALTER TABLE [dbo].[UserChatHistory]
    ADD DEFAULT (GETUTCDATE()) FOR [CreatedDtTm];


GO
PRINT N'Creating On column: CheckinCount...';


GO
ALTER TABLE [dbo].[UserCheckIn]
    ADD DEFAULT (0) FOR [CheckinCount];


GO
PRINT N'Creating On column: UpdatedDtTm...';


GO
ALTER TABLE [dbo].[UserCheckIn]
    ADD DEFAULT (GETUTCDATE()) FOR [UpdatedDtTm];


GO
PRINT N'Creating On column: CheckinCount...';


GO
ALTER TABLE [dbo].[UserCheckInHistory]
    ADD DEFAULT (0) FOR [CheckinCount];


GO
PRINT N'Creating On column: CreatedDtTm...';


GO
ALTER TABLE [dbo].[UserImageHistory]
    ADD DEFAULT (GETUTCDATE()) FOR [CreatedDtTm];


GO
PRINT N'Creating On column: OnlineStatusID...';


GO
ALTER TABLE [dbo].[UserInfo]
    ADD DEFAULT (0) FOR [OnlineStatusID];


GO
PRINT N'Creating On column: UpdatedDtTm...';


GO
ALTER TABLE [dbo].[UserInfo]
    ADD DEFAULT (GETUTCDATE()) FOR [UpdatedDtTm];


GO
PRINT N'Creating On column: CreatedDtTm...';


GO
ALTER TABLE [dbo].[UserInfo]
    ADD DEFAULT (GETUTCDATE()) FOR [CreatedDtTm];


GO
PRINT N'Creating On column: LastLoginDateTime...';


GO
ALTER TABLE [dbo].[UserLogin]
    ADD DEFAULT (GETUTCDATE()) FOR [LastLoginDateTime];


GO
PRINT N'Creating On column: CreatedDtTm...';


GO
ALTER TABLE [dbo].[UserLogin]
    ADD DEFAULT (GETUTCDATE()) FOR [CreatedDtTm];


GO
PRINT N'Creating On column: UpdatedDtTm...';


GO
ALTER TABLE [dbo].[UserSelection]
    ADD DEFAULT (GETUTCDATE()) FOR [UpdatedDtTm];


GO
PRINT N'Creating On column: UpdatedDtTm...';


GO
ALTER TABLE [dbo].[UserSelectionHistory]
    ADD DEFAULT (GETUTCDATE()) FOR [UpdatedDtTm];


GO
PRINT N'Creating [dbo].[CheckOut]...';


GO
CREATE PROCEDURE CheckOut
	@UserId			INT,
	@BusinessID		INT,
	@Gender			SMALLINT = 0,
	@Success		BIT = 0 OUTPUT 
AS
BEGIN
	DECLARE @CurrentBusinessID INT
	SELECT @CurrentBusinessID = 0
	
	SELECT
		@CurrentBusinessID = BusinessId
	FROM UserCheckIn  (NOLOCK) WHERE UserID = @UserID
	
	INSERT INTO UserCheckInHistory
	(
		UserID
		,StartTime
		,EndTime
		,BusinessId
		,CheckinCount
		,UpdatedDtTm
	)
	SELECT
		UserID
		,StartTime
		,GETUTCDATE()
		,BusinessId
		,CheckinCount
		,UpdatedDtTm
	FROM UserCheckIn (NOLOCK) WHERE UserID = @UserID
	AND ISNULL(BusinessId,0) <> 0 
	AND (
			ISNULL(BusinessId,0) <> @BusinessID
		OR
			(BusinessId = @BusinessID AND DATEDIFF(HOUR,UpdatedDtTm,GETUTCDATE()) > 6 )
		)
	
	DECLARE @CheckinId INT = @@IDENTITY

	IF(ISNULL(@CheckinId,0) > 0 )
	BEGIN
		INSERT INTO UserSelectionHistory
		(
			UserCheckInId
			,FromUserID
			,ToUserId
			,StatusId
			,UpdatedDtTm
		)
		SELECT
			@CheckinId,
			FromUserID,
			ToUserId,
			StatusId,
			UpdatedDtTm
		FROM UserSelection (NOLOCK)
		WHERE
			FromUserID = @UserID
	
		SELECT @Success = 1
		
		UPDATE UserCheckIn SET BusinessId = null, StartTime = null WHERE  UserID = @UserID
			IF @Gender = 0
		SELECT 	@Gender = Gender FROM UserInfo (NOLOCK) WHERE UserID = @UserId
			
		IF @Gender = 1
			UPDATE Business SET 
				GenderMCheckIns = GenderMCheckIns - 1, 
				UpdatedDtTm = GETUTCDATE() 
			WHERE  BusinessId = @CurrentBusinessID
			AND GenderMCheckIns > 0
		ELSE
			UPDATE Business SET 
				GenderFCheckIns = GenderFCheckIns - 1, 
				UpdatedDtTm = GETUTCDATE() 
			WHERE  BusinessId = @CurrentBusinessID
					AND GenderFCheckIns > 0
	END
	ELSE
	BEGIN
		IF ISNULL(@CurrentBusinessID,0) = 0
			SELECT @Success = 1
	END
		
END
GO
PRINT N'Creating [dbo].[CreateBusinessLight]...';


GO
CREATE PROCEDURE CreateBusinessLight
	@BusinessLight AS BusinessLight READONLY
AS
BEGIN

	WHILE 1=1
	BEGIN
		BEGIN TRY
		
		INSERT INTO business (externalid,sourceid,externalidCheckSum )
		SELECT a.externalid,a.sourceid,hashbytes('md5',a.externalid) from @BusinessLight a
		LEFT JOIN business  (NOLOCK) b
		ON b.externalidchecksum = hashbytes('md5',a.externalid)
		WHERE b.externalid IS NULL
		
		SELECT b.* from @BusinessLight a
		JOIN business  (NOLOCK) b
		ON a.externalid = b.externalid
		
		BREAK
		
		END TRY
		BEGIN CATCH
			
		END CATCH
	END
END
GO
PRINT N'Creating [dbo].[CreateBusinessLightTEST]...';


GO
CREATE PROCEDURE CreateBusinessLightTEST
	@BusinessLight AS BusinessLight READONLY
AS
BEGIN
	WHILE 1=1
	BEGIN
		BEGIN TRY
		DECLARE @business TABLE (BusinessId INT)

		INSERT INTO business (externalid,sourceid,externalidCheckSum )
		SELECT a.externalid,a.sourceid,hashbytes('md5',a.externalid) from @BusinessLight a
		LEFT JOIN business  (NOLOCK) b
		ON b.externalidchecksum = hashbytes('md5',a.externalid)
		WHERE b.externalid IS NULL
		
		INSERT INTO @business
		SELECT b.BusinessId from @BusinessLight a
		JOIN business  (NOLOCK) b
		ON a.externalid = b.externalid
		
		UPDATE UserCheckIn SET BusinessId = null

		WHILE (SELECT COUNT(1) FROM @business WHERE BusinessId NOT IN (SELECT ISNULL(BusinessId,0) From UserCheckIn (NOLOCK))) > 0
		BEGIN
			UPDATE U
				SET U.BusinessId = (SELECT top 1 BusinessId from @business WHERE BusinessId not in (select ISNULL(BusinessId,0) from UserCheckIn))
				,StartTime = GETUTCDATE()
			FROM UserCheckIn U (NOLOCK)
			JOIN (Select top 10 userid from UserCheckIn (NOLOCK) WHERE BusinessId IS NULL) b
			ON b.UserID = u.UserID;
		END


		
		SELECT b.* from @BusinessLight a
		JOIN business  (NOLOCK) b
		ON a.externalid = b.externalid
		
		BREAK
		
		END TRY
		BEGIN CATCH
			
		END CATCH
	END
END
GO
PRINT N'Creating [dbo].[LoginCreate]...';


GO
CREATE PROCEDURE LoginCreate
	@UserName	nvarchar(10) = NULL,
	@UserPassword nvarchar(20) = NULL ,
	@ExternalId varchar (30) = NULL,
	@SourceID [smallint]
AS
BEGIN
	
	declare @userId Table(userid int, ChangeType VARCHAR(100))
	DECLARE @IsNEW VARCHAR(10)

	
	SELECT @IsNEW = 'false'

	
	
		MERGE dbo.UserLogin AS Target
		USING (SELECT	
					@ExternalId AS Externalid,
					@SourceID AS Sourceid,
					HASHBYTES('MD5',@ExternalId + CAST(@SourceID AS VARCHAR(4))) AS externalidCheckSum,
					0 AS disabled
						) AS Source
			ON Target.externalidCheckSum = Source.externalidCheckSum
		WHEN MATCHED THEN 
			UPDATE SET Target.LastLoginDateTime = GETUTCDATE()
		WHEN NOT MATCHED THEN
			INSERT 
			(
				externalid,
				sourceid,
				externalidCheckSum,
				disabled
			)
			VALUES
			(
				SOURCE.externalid,
				SOURCE.sourceid,
				SOURCE.externalidCheckSum,
				SOURCE.disabled
			)
		OUTPUT	Inserted.UserId, $action into @userId;

	IF (SELECT ChangeType from @userId) = 'INSERT'
	BEGIN
		
		INSERT INTO UserCheckIn
		(UserID)
		SELECT userid from  @UserID
	END

	IF NOT EXISTS(SELECT TOP 1 * FROM UserInfo (NOLOCK) WHERE UserID = (SELECT UserID from @userId))
	BEGIN
		SELECT @IsNEW = 'true'
	END
	
	SELECT *,@IsNEW AS IsNEW from UserLogin  (NOLOCK) WHERE  externalidCheckSum  = HASHBYTES('MD5',@ExternalId + CAST(@SourceID AS VARCHAR(4)))
END
GO
PRINT N'Creating [dbo].[LoginPasswordCreate]...';


GO
CREATE PROCEDURE LoginPasswordCreate
	@UserName	nvarchar(10) = NULL,
	@UserPassword nvarchar(20) = NULL
AS
BEGIN
	
	declare @userId Table(userid int, ChangeType VARCHAR(100))
	DECLARE @Exists VARCHAR(10)
	DECLARE @password NVARCHAR(40)
	
	SELECT @Exists = 'false'
	SELECT @password = NULL

	SELECT @password = UserPassword FROM UserLogin (NOLOCK) WHERE externalidCheckSum = HASHBYTES('MD5',@UserName) AND UserName = @UserName

	IF NOT EXISTS (SELECT TOP 1 * FROM UserLogin (NOLOCK) WHERE externalidCheckSum = HASHBYTES('MD5',@UserName) AND UserName = @UserName)
	BEGIN
		INSERT INTO dbo.UserLogin
		(
			UserName
			,UserPassword
			,ExternalId
			,SourceID
			,ExternalIdCheckSum
			,LastLoginDateTime
			,CreatedDtTm
			,CreatedBy
			,Disabled
		)
		VALUES
		(
			@UserName
			,HASHBYTES('MD5',@UserPassword)
			,0
			,0
			,HASHBYTES('MD5',@UserName)
			,GETUTCDATE()
			,GETUTCDATE()
			,'system'
			,0
		)
		SELECT @@IDENTITY, 'false'
	END
	ELSE
	BEGIN
		SELECT 0, 'true'
	END

END
GO
PRINT N'Creating [dbo].[UserChatAdd]...';


GO
CREATE PROCEDURE UserChatAdd
	@FromUserId	INT,
	@ToUserId INT,
	@Message NVARCHAR (120)
AS
BEGIN
	DECLARE @High INT, 
			@Low INT,
			@IsHigherMessage BIT = 0

	IF @FromUserId > @ToUserId
		SELECT @High = @FromUserId, @Low = @ToUserId
	ELSE
		SELECT @High = @ToUserId, @Low = @FromUserId

		IF @High = @FromUserId
			SELECT @IsHigherMessage = 1
	

	INSERT INTO UserChat
	(
		HigherUserID,
		LowerUserID,
		IsHigherMessage,
		UserMessage
	)
	SELECT
		@High,
		@Low,
		@IsHigherMessage,
		@Message

	SELECT @@IDENTITY AS ChatId

END
GO
PRINT N'Creating [dbo].[UserChatGet]...';


GO
CREATE PROCEDURE UserChatGet
	@FromUserId	INT,
	@ToUserId INT,
	@ChatId	BIGINT = 0,
	@NoOfChat INT = 0
AS
BEGIN
	DECLARE @High INT, 
			@Low INT,
			@IsHigherMessage BIT = 0

	IF @FromUserId > @ToUserId
		SELECT @High = @FromUserId, @Low = @ToUserId
	ELSE
		SELECT @High = @ToUserId, @Low = @FromUserId

	IF @NoOfChat = 0
		SELECT 
			ChatId
			,HigherUserID
			,LowerUserID
			,IsHigherMessage
			,UserMessage
			,CreatedDtTm 
		FROM UserChat (NOLOCK)
		WHERE 
			HigherUserID = @High
			AND LowerUserID = @Low
			AND ChatId > @ChatId
		ORDER BY CreatedDtTm DESC
	ELSE
		SELECT TOP (@NoOfChat)
			ChatId
			,HigherUserID
			,LowerUserID
			,IsHigherMessage
			,UserMessage
			,CreatedDtTm 
		FROM UserChat (NOLOCK)
		WHERE 
			HigherUserID = @High
			AND LowerUserID = @Low
			AND ChatId > @ChatId
		ORDER BY CreatedDtTm DESC

END
GO
PRINT N'Creating [dbo].[UserImageURLUpdate]...';


GO
CREATE PROCEDURE UserImageURLUpdate
	@UserId	INT
	,@ImageURL VARCHAR(8000) = NULL
AS
BEGIN	
	DECLARE @OldImageURL VARCHAR(8000)
	SELECT @OldImageURL = ISNULL(PictureLocation,'') FROM UserInfo (NOLOCK) WHERE UserID = @UserId

	IF @OldImageURL != @ImageURL
	BEGIN
		UPDATE UserInfo SET PictureLocation = @ImageURL WHERE UserID = @UserId
		INSERT INTO UserImageHistory
		(UserID, PictureLocation)
		VALUES 
		(@UserId, @OldImageURL)
	END
END
GO
PRINT N'Creating [dbo].[UserInfoCreate]...';


GO
CREATE PROCEDURE UserInfoCreate
	@UserId				int,
--	@PictureLocation	varchar(800) = NULL,
	@Gender				smallint,
	@Preference			smallint,
	@Age				smallint,
	@Status				smallint = 1,
	@Email				nvarchar(140) = NULL,
	@Handle				nvarchar(60) = NULL
AS
BEGIN
	IF EXISTS(SELECT * FROM UserLogin (NOLOCK) WHERE UserID = @UserId)
	BEGIN
		;MERGE dbo.UserInfo AS Target
		USING (SELECT	@UserId AS UserID ,
						--@PictureLocation AS PictureLocation, 
						@Gender AS Gender, 
						@Preference AS Preference, 
						@Age AS Age, 
						@Status AS OnlineStatusID,
						@Email AS Email,
						@Handle AS Handle
						) AS Source
			ON Target.UserID = Source.UserID
		WHEN MATCHED THEN UPDATE SET
			--Target.PictureLocation = ISNULL(Source.PictureLocation,Target.PictureLocation)
			Target.Gender = Source.Gender,
			Target.Preference = Source.Preference,
			Target.Age = Source.Age,
			Target.Email = Source.Email,
			Target.Handle = Source.Handle,
			Target.UpdatedDtTm = GETUTCDATE()
		WHEN NOT MATCHED THEN
		INSERT 
			(
				UserID,
				--PictureLocation,
				Gender,
				Preference,
				Age,
				OnlineStatusID,
				Email,
				Handle
			)
			VALUES
			(
				SOURCE.UserID,
				--SOURCE.PictureLocation,
				SOURCE.Gender,
				SOURCE.Preference,
				SOURCE.Age,
				SOURCE.OnlineStatusID,
				SOURCE.Email,
				SOURCE.Handle
			) ;
	END
	ELSE
	BEGIN
		RAISERROR ( N'INVALID %s %d.', 18, 1, 'UserId', @UserId);
	END
END
GO
PRINT N'Creating [dbo].[UserInfoGet]...';


GO
CREATE PROCEDURE UserInfoGet
	@UserId	INT
AS
BEGIN	
	SELECT * from UserInfo (NOLOCK) WHERE  UserID  = @UserId
END
GO
PRINT N'Creating [dbo].[UserOnlineStatusUpdate]...';


GO
CREATE PROCEDURE UserOnlineStatusUpdate
	@UserId	INT
	,@StatusId SMALLINT
AS
BEGIN	
	UPDATE UserInfo SET OnlineStatusID = @StatusId WHERE UserID = @UserId
END
GO
PRINT N'Creating [dbo].[UserSelectionGet]...';


GO
CREATE PROCEDURE UserSelectionGet
	@UserId	INT,
	@TypeId	SMALLINT = 0 /* 0=ALL, 1=LIKES, 2=DISLIKES, 3=HITS (order by touser likes) 4=MISSES (order by touser likes) 5=My choices 6=others choices*/
AS
BEGIN	
	
	IF @TypeId = 1
		SELECT ToUserId AS ToUserId FROM UserSelection (NOLOCK)
		WHERE 
			FromUserID = @UserId
			AND StatusId = 1
	ELSE IF @TypeId = 2
		SELECT ToUserId ToUserId FROM UserSelection (NOLOCK)
		WHERE 
			FromUserID = @UserId
			AND StatusId = 0
	ELSE IF @TypeId = 3
		SELECT FromUserID ToUserId FROM UserSelection (NOLOCK)
		WHERE 
			FromUserID IN 
				(SELECT ToUserId FROM UserSelection (NOLOCK)
				WHERE FromUserID = @UserId AND StatusId = 1)
			AND ToUserId = @UserId
			AND StatusId = 1
	ELSE IF @TypeId = 4
		SELECT FromUserID ToUserId FROM UserSelection (NOLOCK)
		WHERE 
			FromUserID IN 
				(SELECT ToUserId FROM UserSelection (NOLOCK)
				WHERE FromUserID = @UserId AND StatusId = 0)
			AND ToUserId = @UserId
			AND StatusId = 1
	ELSE IF @TypeId = 5
		SELECT ToUserId AS ToUserId, StatusId FROM UserSelection (NOLOCK)
		WHERE 
			FromUserID = @UserId
	ELSE IF @TypeId = 6
		SELECT FromUserID AS ToUserId, StatusId FROM UserSelection (NOLOCK)
		WHERE 
			ToUserId = @UserId
			
END
GO
PRINT N'Creating [dbo].[UserSelectionUpdate]...';


GO
CREATE PROCEDURE UserSelectionUpdate
	@FromUserId			INT,
	@ToUserId			INT,
	@Statusid			SMALLINT = 0
AS
BEGIN	

	;MERGE dbo.UserSelection AS Target
	USING (SELECT @FromUserId AS FromUserId , @ToUserId AS ToUserId) AS Source
		ON Target.FromUserId = Source.FromUserId
		AND Target.ToUserId = Source.ToUserId
	WHEN MATCHED THEN UPDATE SET
		Target.Statusid = @Statusid,
		Target.UpdatedDtTm = GETUTCDATE()
	WHEN NOT MATCHED THEN
	INSERT 
		(
			FromUserId,
			ToUserId,
			Statusid,
			UpdatedDtTm
		)
		VALUES
		(
			SOURCE.FromUserId,
			SOURCE.ToUserId,
			@Statusid,
			GETUTCDATE()
		) ;
			
END
GO
PRINT N'Creating [dbo].[UserTitleUpdate]...';


GO
CREATE PROCEDURE UserTitleUpdate
	@UserId	INT
	,@Title NVARCHAR(140)
AS
BEGIN	
	UPDATE UserInfo SET InstantTitle = @Title WHERE UserID = @UserId
END
GO
PRINT N'Creating [dbo].[VisibleProfilesByBusinessGet]...';


GO
CREATE PROCEDURE VisibleProfilesByBusinessGet
	@UserId	INT
AS
BEGIN	
	DECLARE @BusinessID INT
			,@Preference SMALLINT
			
	SELECT	@BusinessID = 0
			,@Preference = 0 
	
	SELECT @BusinessID = BusinessId FROM UserCheckIn (NOLOCK) WHERE UserID = @UserId
	SELECT @Preference = Preference FROM UserInfo (NOLOCK) WHERE UserID = @UserId
	
	IF @Preference = 3
		SELECT u.* FROM UserInfo (NOLOCK) u
		JOIN UserCheckIn (NOLOCK) uc
		ON uc.UserID = u.UserID
		WHERE
			uc.BusinessId = @BusinessID
			AND u.OnlineStatusID > 0
			AND uc.UserID <> @UserId
			
	ELSE
		SELECT u.* FROM UserInfo (NOLOCK) u
		JOIN UserCheckIn (NOLOCK) uc
		ON uc.UserID = u.UserID
		WHERE
			uc.BusinessId = @BusinessID
			AND u.Gender = @Preference
			AND u.OnlineStatusID > 0
			AND uc.UserID <> @UserId
END
GO
PRINT N'Creating [dbo].[AutoCheckout]...';


GO
CREATE PROCEDURE AutoCheckout
AS
BEGIN
	
	DECLARE @users TABLE (userid INT, isprocessed bit)
	DECLARE @count INT
			,@userid INT
	
	INSERT INTO @users
	SELECT
		UserID
		,0
	FROM UserCheckIn (NOLOCK)
	WHERE ISNULL(BusinessId,0) <> 0 
	AND DATEDIFF(HOUR,UpdatedDtTm,GETUTCDATE()) > 6 
	
	SELECT @count = COUNT(1) FROM @users WHERE isprocessed = 0
	WHILE @count > 0
	BEGIN
		
		SELECT TOP 1 @userid = userid from @users WHERE isprocessed = 0
		
		EXEC CheckOut @UserId = @userid , @BusinessID = 0

		UPDATE @users SET isprocessed = 1 WHERE userid = @userid
		
		SELECT @count = 0
		SELECT @count = COUNT(1) FROM @users WHERE isprocessed = 0
		
	END

END
GO
PRINT N'Creating [dbo].[CheckIn]...';


GO
CREATE PROCEDURE CheckIn
	@UserId			int,
	@BusinessId		int
AS
BEGIN
	
	DECLARE @Gender SMALLINT
	DECLARE @Success BIT
	SELECT @Success = 0
	
	SELECT 	@Gender = Gender FROM UserInfo (NOLOCK) WHERE UserID = @UserId
	
	EXEC CheckOut @UserId = @UserId, @BusinessId = @BusinessId , @Gender = @Gender, @Success = @Success OUTPUT
	
	UPDATE UserCheckIn SET 
		BusinessId = @BusinessId, 
		CheckinCount = CASE WHEN BusinessId = @BusinessId THEN CheckinCount ELSE CheckinCount + 1 END,
		StartTime = GETUTCDATE(), 
		UpdatedDtTm = GETUTCDATE() 
	WHERE  UserID = @UserID
	
	IF @Success = 1
	BEGIN
		IF @Gender = 1
			UPDATE Business SET 
				GenderMCheckIns = GenderMCheckIns + 1, 
				UpdatedDtTm = GETUTCDATE() 
			WHERE  BusinessId = @BusinessId
		ELSE
			UPDATE Business SET 
				GenderFCheckIns = GenderFCheckIns + 1, 
				UpdatedDtTm = GETUTCDATE() 
			WHERE  BusinessId = @BusinessId
	END
END
GO
-- Refactoring step to update target server with deployed transaction logs
CREATE TABLE  [dbo].[__RefactorLog] (OperationKey UNIQUEIDENTIFIER NOT NULL PRIMARY KEY)
GO
sp_addextendedproperty N'microsoft_database_tools_support', N'refactoring log', N'schema', N'dbo', N'table', N'__RefactorLog'
GO

GO
/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        DECLARE @VarDecimalSupported AS BIT;
        SELECT @VarDecimalSupported = 0;
        IF ((ServerProperty(N'EngineEdition') = 3)
            AND (((@@microsoftversion / power(2, 24) = 9)
                  AND (@@microsoftversion & 0xffff >= 3024))
                 OR ((@@microsoftversion / power(2, 24) = 10)
                     AND (@@microsoftversion & 0xffff >= 1600))))
            SELECT @VarDecimalSupported = 1;
        IF (@VarDecimalSupported > 0)
            BEGIN
                EXECUTE sp_db_vardecimal_storage_format N'$(DatabaseName)', 'ON';
            END
    END


GO
ALTER DATABASE [$(DatabaseName)]
    SET MULTI_USER 
    WITH ROLLBACK IMMEDIATE;


GO
