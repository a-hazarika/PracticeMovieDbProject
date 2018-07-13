USE MovieDatabase
GO

/****** Object:  Table [dbo].[MovieProducerMappings]    Script Date: 7/12/2018 9:03:15 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MovieProducerMappings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MovieId] [int] NULL,
	[ProducerId] [int] NULL,
 CONSTRAINT [PK_MovieProducerMappings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[MovieProducerMappings]  WITH CHECK ADD  CONSTRAINT [FK_MovieProducerMappings_Movies_MovieId] FOREIGN KEY([MovieId])
REFERENCES [dbo].[Movies] ([Id])
GO

ALTER TABLE [dbo].[MovieProducerMappings] CHECK CONSTRAINT [FK_MovieProducerMappings_Movies_MovieId]
GO

ALTER TABLE [dbo].[MovieProducerMappings]  WITH CHECK ADD  CONSTRAINT [FK_MovieProducerMappings_Producers_ProducerId] FOREIGN KEY([ProducerId])
REFERENCES [dbo].[Producers] ([Id])
GO

ALTER TABLE [dbo].[MovieProducerMappings] CHECK CONSTRAINT [FK_MovieProducerMappings_Producers_ProducerId]
GO

