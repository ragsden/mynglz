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

