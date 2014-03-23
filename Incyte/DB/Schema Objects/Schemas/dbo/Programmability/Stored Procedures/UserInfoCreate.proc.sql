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