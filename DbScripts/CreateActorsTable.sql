USE MovieDatabase
GO

/****** Object:  Table [dbo].[Actors]    Script Date: 7/12/2018 9:02:52 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Actors](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](255) NOT NULL,
	[MiddleName] [nvarchar](255) NULL,
	[LastName] [nvarchar](255) NOT NULL,
	[SexId] [int] NOT NULL,
	[Bio] [nvarchar](max) NULL,
	[DOB] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Actors] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Actors]  WITH CHECK ADD  CONSTRAINT [FK_Actors_Gender_SexId] FOREIGN KEY([SexId])
REFERENCES [dbo].[Gender] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Actors] CHECK CONSTRAINT [FK_Actors_Gender_SexId]
GO

