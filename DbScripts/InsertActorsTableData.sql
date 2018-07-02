SET IDENTITY_INSERT [dbo].[Actors] ON
INSERT INTO [dbo].[Actors] ([Id], [FirstName], [MiddleName], [LastName], [Bio], [DOB], [SexId]) VALUES (2, N'Chris', NULL, N'Pratt', N'Christopher Michael Pratt is an American film and television actor.', N'1979-06-21 00:00:00', 1)
INSERT INTO [dbo].[Actors] ([Id], [FirstName], [MiddleName], [LastName], [Bio], [DOB], [SexId]) VALUES (3, N'Bryce', N'Dallas', N'Howard', N'Bryce Dallas Howard was born on March 2, 1981, in Los Angeles, California. She was conceived in Dallas, Texas (the reason for her middle name). Her father, Ron Howard, is a former actor turned Oscar-winning director.', N'1981-03-02 00:00:00', 2)
SET IDENTITY_INSERT [dbo].[Actors] OFF
