using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PcBuilderBackend.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRamRank : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'ram_rank') THEN
                        CREATE TYPE ram_rank AS ENUM ('single_rank', 'dual_rank');
                    END IF;
                END $$");

            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    IF NOT EXISTS (
                        SELECT 1 FROM information_schema.columns
                        WHERE table_name = 'Rams' AND column_name = 'RamRank'
                    ) THEN
                        ALTER TABLE ""Rams"" ADD ""RamRank"" ram_rank NOT NULL DEFAULT 'single_rank'::ram_rank;
                    END IF;
                END $$");

            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    IF NOT EXISTS (
                        SELECT 1 FROM information_schema.columns
                        WHERE table_name = 'CpuRamMaxSpeed' AND column_name = 'RamRank'
                    ) THEN
                        ALTER TABLE ""CpuRamMaxSpeed"" ADD ""RamRank"" ram_rank NOT NULL DEFAULT 'single_rank'::ram_rank;
                    END IF;
                END $$");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    IF EXISTS (
                        SELECT 1 FROM information_schema.columns
                        WHERE table_name = 'CpuRamMaxSpeed' AND column_name = 'RamRank'
                    ) THEN
                        ALTER TABLE ""CpuRamMaxSpeed"" DROP COLUMN ""RamRank"";
                    END IF;
                END $$");

            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    IF EXISTS (
                        SELECT 1 FROM information_schema.columns
                        WHERE table_name = 'Rams' AND column_name = 'RamRank'
                    ) THEN
                        ALTER TABLE ""Rams"" DROP COLUMN ""RamRank"";
                    END IF;
                END $$");

            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    IF EXISTS (SELECT 1 FROM pg_type WHERE typname = 'ram_rank') THEN
                        DROP TYPE ram_rank;
                    END IF;
                END $$");
        }
    }
}
