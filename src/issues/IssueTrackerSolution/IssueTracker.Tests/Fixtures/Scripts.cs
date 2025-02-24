namespace IssueTracker.Tests.Fixtures;

public static class Scripts
{
    public const string SeedCatalog = """
                                      CREATE TABLE IF NOT EXISTS catalog (
                                         id uuid PRIMARY KEY,
                                         title varchar(50) NOT NULL,
                                         description varchar(100) NOT NULL,
                                         vendor varchar(10) NOT NULL
                                      );

                                      INSERT INTO "catalog" ("id", "title", "description", "vendor") VALUES
                                      ('3497e4fc-5308-487a-9148-121520d5b1dd',	'Visual Studio Code',	'Editor for Programmers',	'Microsoft'),
                                      ('04d269e5-d5a1-4281-a812-d066b415e3c0',	'Rider',	'Integrated development environment for .NET Developers',	'Jetbrains'),
                                      ('7d089f74-4945-4c9a-bf1f-f952b293b3ec',	'Docker Desktop',	'Container Runtime, Builder, and UI ',	'Docker'),
                                      ('4f2ba3f7-8aaf-42ba-ac3e-a1a56fe82096',	'Visual Studio',	'IDE For Windows developers',	'Microsoft');
                                      """;
    
}

public static class SeededSoftware
{
    public const string DockerDesktop = "7d089f74-4945-4c9a-bf1f-f952b293b3ec";
    public const string VisualStudioCode = "3497e4fc-5308-487a-9148-121520d5b1dd";
    public const string Rider = "04d269e5-d5a1-4281-a812-d066b415e3c0";
    public const string VisualStudio = "4f2ba3f7-8aaf-42ba-ac3e-a1a56fe82096";
}