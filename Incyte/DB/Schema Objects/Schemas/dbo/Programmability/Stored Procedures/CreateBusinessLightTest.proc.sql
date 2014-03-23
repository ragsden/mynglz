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


