CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20230430164331_InitialCreate') THEN
    CREATE TABLE "MatchResults" (
        "Id" uuid NOT NULL,
        "Completed" timestamp with time zone NOT NULL,
        "DidTeam1Win" boolean NOT NULL,
        "Team1Members" text NOT NULL,
        "Team2Members" text NOT NULL,
        CONSTRAINT "PK_MatchResults" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20230430164331_InitialCreate') THEN
    CREATE TABLE "Players" (
        "Id" uuid NOT NULL,
        "UserId" integer NOT NULL,
        "Wins" integer NOT NULL,
        "Losses" integer NOT NULL,
        "Elo" integer NOT NULL,
        "HighestElo" integer NOT NULL,
        "LongestWinstreak" integer NOT NULL,
        "LongestLossstreak" integer NOT NULL,
        "CurrentWinstreak" integer NOT NULL,
        "CurrentLossstreak" integer NOT NULL,
        CONSTRAINT "PK_Players" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20230430164331_InitialCreate') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20230430164331_InitialCreate', '7.0.5');
    END IF;
END $EF$;
COMMIT;

