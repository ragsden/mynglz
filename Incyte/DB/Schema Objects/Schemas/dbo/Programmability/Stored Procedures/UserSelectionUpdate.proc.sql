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

