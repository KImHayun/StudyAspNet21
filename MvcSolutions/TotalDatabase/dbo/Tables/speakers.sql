CREATE TABLE [dbo].[speakers] (
    [SpeakerId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (MAX) NOT NULL,
    [RoomId]    INT            NOT NULL,
    CONSTRAINT [PK_speakers] PRIMARY KEY CLUSTERED ([SpeakerId] ASC),
    CONSTRAINT [FK_speakers_Rooms_RoomId] FOREIGN KEY ([RoomId]) REFERENCES [dbo].[Rooms] ([RoomId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_speakers_RoomId]
    ON [dbo].[speakers]([RoomId] ASC);

