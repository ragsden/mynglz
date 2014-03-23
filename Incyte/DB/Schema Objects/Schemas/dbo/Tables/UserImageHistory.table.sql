
CREATE TABLE [dbo].[UserImageHistory](
	[UserID] [int] NOT NULL,
	[PictureLocation] [varchar](8000) NULL,
	[CreatedDtTm] [datetime] NOT NULL DEFAULT(GETUTCDATE())
)

