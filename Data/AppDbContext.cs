using System;
using GaryPortalAPI.Models;
using GaryPortalAPI.Models.Feed;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


    public DbSet<User> Users { get; set; }
    public DbSet<Rank> Ranks { get; set; }
    public DbSet<Team> Teams { get; set; }

    public DbSet<UserAuthentication> UserAuthentications { get; set; }
    public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
    public DbSet<UserPoints> UserPoints { get; set; }
    public DbSet<UserRanks> UserRanks { get; set; }
    //public DbSet<UserBans> UserBans { get; set; }
    public DbSet<UserTeam> UserTeams { get; set; }

    public DbSet<FeedPost> FeedPosts { get; set; }
    public DbSet<FeedMediaPost> FeedMediaPosts { get; set; }
    public DbSet<FeedPollPost> FeedPollPosts { get; set; }
    public DbSet<FeedLike> FeedPostLikes { get; set; }
    public DbSet<FeedComment> FeedPostComments { get; set; }
    public DbSet<FeedAnswerVote> FeedAnswerVotes { get; set; }
    public DbSet<FeedPollAnswer> FeedPollAnswers { get; set; }
    public DbSet<AditLog> FeedAditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region User

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.UserUUID);
            entity.Ignore(u => u.UserAuthTokens);

            entity
                .HasOne(u => u.UserAuthentication)
                .WithOne(ua => ua.User)
                .HasForeignKey<UserAuthentication>(ua => ua.UserUUID)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity
                .HasMany(u => u.UserRefreshTokens)
                .WithOne(urt => urt.User)
                .HasForeignKey(urt => urt.UserUUID)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity
                .HasOne(u => u.UserPoints)
                .WithOne(u => u.User)
                .HasForeignKey<UserPoints>(up => up.UserUUID)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity
                .HasOne(u => u.UserRanks)
                .WithOne(ur => ur.User)
                .HasForeignKey<UserRanks>(ur => ur.UserUUID)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            // TODO: Bans architecture

            entity
                .HasOne(u => u.UserTeam)
                .WithOne(ut => ut.User)
                .HasForeignKey<UserTeam>(ut => ut.UserUUID)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });

        modelBuilder.Entity<UserAuthentication>(entity =>
        {
            entity.HasKey(ua => ua.UserUUID);
        });

        modelBuilder.Entity<UserRefreshToken>(entity =>
        {
            entity.HasKey(urt => new { urt.UserUUID, urt.RefreshToken });
        });

        modelBuilder.Entity<UserRanks>(entity =>
        {
            entity.HasKey(ur => ur.UserUUID);
            entity
                .HasOne(ur => ur.AmigoRank)
                .WithMany(r => r.UserAmigoRanks)
                .HasForeignKey(ur => ur.AmigoRankId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity
                .HasOne(ur => ur.PositivityRank)
                .WithMany(r => r.UserPositivityRanks)
                .HasForeignKey(ur => ur.PositivtyRankId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();      
        });

        modelBuilder.Entity<UserPoints>(entity =>
        {
            entity.HasKey(up => up.UserUUID);
        });

        modelBuilder.Entity<UserTeam>(entity =>
        {
            entity.HasKey(ut => ut.UserUUID);
        });

        #endregion User

        #region Feed

        modelBuilder.Entity<FeedPost>(entity =>
        {
            entity.Property(fp => fp.PostId)
                .ValueGeneratedOnAdd();
            entity.HasKey(fp => fp.PostId);
            entity.Ignore(fp => fp.PosterDTO);
            entity.HasDiscriminator(fp => fp.PostType)
                .HasValue<FeedPost>("base")
                .HasValue<FeedMediaPost>("media")
                .HasValue<FeedPollPost>("poll");

            entity
                .HasOne(fp => fp.Poster)
                .WithMany(u => u.FeedPosts)
                .HasForeignKey(fp => fp.PosterUUID)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity
                .HasOne(fp => fp.PostTeam)
                .WithMany(ut => ut.FeedPosts)
                .HasForeignKey(fp => fp.TeamId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity
                .HasMany(fp => fp.Likes)
                .WithOne(l => l.Post)
                .HasForeignKey(l => l.PostId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity
                .HasMany(fp => fp.Comments)
                .WithOne(c => c.Post)
                .HasForeignKey(c => c.PostId)
                .IsRequired();
        });

        modelBuilder.Entity<FeedPollPost>(entity =>
        {
            entity
                .HasMany(fpp => fpp.PollAnswers)
                .WithOne(pa => pa.Poll)
                .HasForeignKey(pa => pa.PollId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });

        modelBuilder.Entity<FeedPollAnswer>(entity =>
        {
            entity.Property(fpa => fpa.PollAnswerId)
                .ValueGeneratedOnAdd();
            entity.HasKey(fpa => fpa.PollAnswerId);
            entity
                .HasMany(fpa => fpa.Votes)
                .WithOne(fav => fav.PollAnswer)
                .HasForeignKey(fav => fav.PollAnswerId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });

        modelBuilder.Entity<FeedAnswerVote>(entity =>
        {
            entity.HasKey(fav => new { fav.PollAnswerId, fav.UserUUID });
            entity.Ignore(fav => fav.UserDTO);
            entity
                .HasOne(fav => fav.User)
                .WithMany(u => u.FeedVotes)
                .HasForeignKey(fav => fav.UserUUID)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });

        modelBuilder.Entity<FeedLike>(entity =>
        {
            entity.HasKey(fl => new { fl.PostId, fl.UserUUID });
            entity.Ignore(fl => fl.UserDTO);
            entity
                   .HasOne(fl => fl.User)
                   .WithMany(u => u.FeedLikes)
                   .HasForeignKey(fl => fl.UserUUID)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();

        });

        modelBuilder.Entity<FeedComment>(entity =>
        {
            entity.Property(fc => fc.FeedCommentId)
                .ValueGeneratedOnAdd();
            entity.Ignore(fc => fc.UserDTO);
            entity
                   .HasOne(fc => fc.User)
                   .WithMany(u => u.FeedComments)
                   .HasForeignKey(fc => fc.UserUUID)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();

        });

        modelBuilder.Entity<AditLog>(entity =>
        {
            entity.Property(al => al.AditLogId)
                .ValueGeneratedOnAdd();
            entity.Ignore(al => al.PosterDTO);
            entity
                .HasOne(al => al.Poster)
                .WithMany(p => p.AditLogs)
                .HasForeignKey(al => al.PosterUUID)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
            entity
                .HasOne(al => al.AditLogTeam)
                .WithMany(t => t.AditLogs)
                .HasForeignKey(al => al.AditLogTeamId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });

        #endregion
    }
}