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