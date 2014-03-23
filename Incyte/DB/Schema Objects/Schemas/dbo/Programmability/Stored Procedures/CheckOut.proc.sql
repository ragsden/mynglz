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

