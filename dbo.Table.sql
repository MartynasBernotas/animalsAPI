CREATE TABLE [dbo].[Animal]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Animal] TEXT NOT NULL, 
    [hasFur] BIT NULL, 
    [isBird] BIT NULL, 
    [eatsFruit] BIT NULL, 
    [huntsRabbit] BIT NULL
)
