/****** Script for SelectTopNRows command from SSMS  ******/
SELECT m.Name, m.ReleaseYear, p.FirstName as "Producer", a.FirstName as "Actor"
  FROM Movies m
  inner join Producers p on m.ProducerId = p.Id
  inner join MovieActorMappings map on m.Id = map.MovieId
  inner join Actors a on a.Id = map.ActorId and m.Id = map.MovieId