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
