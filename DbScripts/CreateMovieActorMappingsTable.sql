
/****** Object:  Table [dbo].[MovieActorMappings]    Script Date: 7/12/2018 9:22:47 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MovieActorMappings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MovieId] [int] NOT NULL,
	[ActorId] [int] NOT NULL,
 CONSTRAINT [PK_MovieActorMappings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[MovieActorMappings]  WITH CHECK ADD  CONSTRAINT [FK_MovieActorMappings_Actors_ActorId] FOREIGN KEY([ActorId])
REFERENCES [dbo].[Actors] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[MovieActorMappings] CHECK CONSTRAINT [FK_MovieActorMappings_Actors_ActorId]
GO

ALTER TABLE [dbo].[MovieActorMappings]  WITH CHECK ADD  CONSTRAINT [FK_MovieActorMappings_Movies_MovieId] FOREIGN KEY([MovieId])
REFERENCES [dbo].[Movies] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[MovieActorMappings] CHECK CONSTRAINT [FK_MovieActorMappings_Movies_MovieId]
GO

