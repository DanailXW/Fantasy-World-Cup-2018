using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FantasyCup.Model
{
    public partial class FantasyCupContext : DbContext
    {
        public virtual DbSet<BetType> BetType { get; set; }
        public virtual DbSet<Competition> Competition { get; set; }
        public virtual DbSet<CompetitionUserBet> CompetitionUserBet { get; set; }
        public virtual DbSet<Game> Game { get; set; }
        public virtual DbSet<GameState> GameState { get; set; }
        public virtual DbSet<GameUserBet> GameUserBet { get; set; }
        public virtual DbSet<Goal> Goal { get; set; }
        public virtual DbSet<GoalType> GoalType { get; set; }
        public virtual DbSet<League> League { get; set; }
        public virtual DbSet<LeagueUser> LeagueUser { get; set; }
        public virtual DbSet<Player> Player { get; set; }
        public virtual DbSet<Result> Result { get; set; }
        public virtual DbSet<ResultCombination> ResultCombination { get; set; }
        public virtual DbSet<ResultType> ResultType { get; set; }
        public virtual DbSet<Stage> Stage { get; set; }
        public virtual DbSet<StageType> StageType { get; set; }
        public virtual DbSet<Team> Team { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserBet> UserBet { get; set; }

        //custom types
        public virtual DbSet<UserScore> UserScore { get; set; }
        public virtual DbSet<GroupStandings> GroupStandings { get; set; }
        public virtual DbSet<GameUserBetAssoc> GameUserBetAssoc { get; set; }

        public FantasyCupContext() { }

        public FantasyCupContext(DbContextOptions<FantasyCupContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BetType>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<Competition>(entity =>
            {
                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.StartDate).HasColumnType("date");
            });

            modelBuilder.Entity<CompetitionUserBet>(entity =>
            {
                entity.Property(e => e.PlaceDate).HasColumnType("datetime");

                entity.HasOne(d => d.BetType)
                    .WithMany(p => p.CompetitionUserBet)
                    .HasForeignKey(d => d.BetTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompetitionUserBet_BetType");

                entity.HasOne(d => d.Competition)
                    .WithMany(p => p.CompetitionUserBet)
                    .HasForeignKey(d => d.CompetitionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompetitionUserBet_Competition");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CompetitionUserBet)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompetitionUserBet_User");
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.TeamAid).HasColumnName("TeamAId");

                entity.Property(e => e.TeamBid).HasColumnName("TeamBId");

                entity.HasOne(d => d.Stage)
                    .WithMany(p => p.Game)
                    .HasForeignKey(d => d.StageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Game_Stage");

                entity.HasOne(d => d.State)
                    .WithMany(p => p.Game)
                    .HasForeignKey(d => d.StateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Game_GameState");

                entity.HasOne(d => d.TeamA)
                    .WithMany(p => p.GameTeamA)
                    .HasForeignKey(d => d.TeamAid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Game_TeamA");

                entity.HasOne(d => d.TeamB)
                    .WithMany(p => p.GameTeamB)
                    .HasForeignKey(d => d.TeamBid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Game_TeamB");
            });

            modelBuilder.Entity<GameState>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GameUserBet>(entity =>
            {
                entity.Property(e => e.PlaceDate).HasColumnType("datetime");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.GameUserBet)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GameUserBet_Game");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.GameUserBet)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GameUserBet_User");

                entity.HasOne(d => d.WinningTeam)
                    .WithMany(p => p.GameUserBet)
                    .HasForeignKey(d => d.WinningTeamId)
                    .HasConstraintName("FK_GameUserBet_Team");
            });

            modelBuilder.Entity<Goal>(entity =>
            {
                entity.Property(e => e.Minute)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.Goal)
                    .HasForeignKey(d => d.PlayerId)
                    .HasConstraintName("FK_Goal_Player");

                entity.HasOne(d => d.Result)
                    .WithMany(p => p.Goal)
                    .HasForeignKey(d => d.ResultId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Goal_Result");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Goal)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK_Goal_GoalType");
            });

            modelBuilder.Entity<GoalType>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<League>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PotAmount).HasColumnType("money");
            });

            modelBuilder.Entity<LeagueUser>(entity =>
            {
                entity.HasKey(e => new { e.LeagueId, e.UserId });

                entity.HasOne(d => d.League)
                    .WithMany(p => p.LeagueUser)
                    .HasForeignKey(d => d.LeagueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LeagueUser_League");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.LeagueUser)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LeagueUser_User");
            });

            modelBuilder.Entity<Player>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.Player)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Player_Team");
            });

            modelBuilder.Entity<Result>(entity =>
            {
                entity.HasOne(d => d.Game)
                    .WithMany(p => p.Result)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Result_Game");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Result)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Result_ResultType");
            });

            modelBuilder.Entity<ResultType>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Stage>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Competition)
                    .WithMany(p => p.Stage)
                    .HasForeignKey(d => d.CompetitionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Stage_Competition");

                entity.HasOne(d => d.StageType)
                    .WithMany(p => p.Stage)
                    .HasForeignKey(d => d.StageTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Stage_StageType");
            });

            modelBuilder.Entity<StageType>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.Property(e => e.EmblemPath).HasMaxLength(500);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.EmailAddress)
                    .IsRequired()
                    .HasMaxLength(600);

                entity.Property(e => e.PasswordHash).IsRequired();

                entity.Property(e => e.PasswordSalt).IsRequired();

                entity.Property(e => e.RefreshToken)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<UserBet>(entity =>
            {
                entity.Property(e => e.PlaceDate).HasColumnType("datetime");

                entity.HasOne(d => d.BetType)
                    .WithMany(p => p.UserBet)
                    .HasForeignKey(d => d.BetTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserBet_BetType");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserBet)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserBet_User");
            });

            modelBuilder.Entity<UserScore>(entity => { entity.HasKey(e => e.Id); });

            modelBuilder.Entity<GroupStandings>(entity =>
            {
                entity.HasKey(e => e.TeamId);

                entity.HasOne(d => d.Team).WithMany(p => p.GroupStandings).HasForeignKey(d => d.TeamId);
                entity.HasOne(d => d.Stage).WithMany(p => p.GroupStandings).HasForeignKey(d => d.StageId);
            });

            modelBuilder.Entity<GameUserBetAssoc>(entity =>
            {
                entity.HasKey(e => new { e.GameId, e.UserId });

                entity.HasOne(d => d.Game).WithMany(p => p.GameUserBetAssoc).HasForeignKey(d => d.GameId);
                entity.HasOne(d => d.User).WithMany(p => p.GameUserBetAssoc).HasForeignKey(d => d.UserId);
            });
        }
    }
}
