USE TodoApp

DELETE FROM Taken

INSERT INTO Taken(Omschrijving, Afgerond, Deadline)
VALUES ('Ontbijten', 0, '2021-12-31'),
	   ('Winkelen', 0, '2022-01-05'),		
	   ('Afwassen', 1, '2021-12-23'),
	   ('Taart bakken', 0, '2021-12-01'),
	   ('Slapen', 1, '2021-12-02');


