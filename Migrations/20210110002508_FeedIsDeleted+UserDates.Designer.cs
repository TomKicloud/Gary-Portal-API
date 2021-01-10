﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GaryPortalAPI.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20210110002508_FeedIsDeleted+UserDates")]
    partial class FeedIsDeletedUserDates
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("GaryPortalAPI.Models.Feed.FeedAnswerVote", b =>
                {
                    b.Property<int>("PollAnswerId")
                        .HasColumnType("int");

                    b.Property<string>("UserUUID")
                        .HasColumnType("varchar(255)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("PollAnswerId", "UserUUID");

                    b.HasIndex("UserUUID");

                    b.ToTable("FeedAnswerVote");
                });

            modelBuilder.Entity("GaryPortalAPI.Models.Feed.FeedComment", b =>
                {
                    b.Property<int>("FeedCommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Comment")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("DatePosted")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsAdminComment")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.Property<string>("UserUUID")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("FeedCommentId");

                    b.HasIndex("PostId");

                    b.HasIndex("UserUUID");

                    b.ToTable("FeedPostComments");
                });

            modelBuilder.Entity("GaryPortalAPI.Models.Feed.FeedLike", b =>
                {
                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.Property<string>("UserUUID")
                        .HasColumnType("varchar(255)");

                    b.Property<bool>("IsLiked")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("PostId", "UserUUID");

                    b.HasIndex("UserUUID");

                    b.ToTable("FeedPostLikes");
                });

            modelBuilder.Entity("GaryPortalAPI.Models.Feed.FeedPollAnswer", b =>
                {
                    b.Property<int>("PollAnswerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Answer")
                        .HasColumnType("longtext");

                    b.Property<int>("PollId")
                        .HasColumnType("int");

                    b.HasKey("PollAnswerId");

                    b.HasIndex("PollId");

                    b.ToTable("FeedPollAnswer");
                });

            modelBuilder.Entity("GaryPortalAPI.Models.Feed.FeedPost", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("PostCreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("PostDescription")
                        .HasColumnType("longtext");

                    b.Property<int>("PostLikeCount")
                        .HasColumnType("int");

                    b.Property<string>("PostType")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PosterUUID")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<int>("TeamId")
                        .HasColumnType("int");

                    b.HasKey("PostId");

                    b.HasIndex("PosterUUID");

                    b.HasIndex("TeamId");

                    b.ToTable("FeedPosts");

                    b.HasDiscriminator<string>("PostType").HasValue("FeedPost");
                });

            modelBuilder.Entity("GaryPortalAPI.Models.Rank", b =>
                {
                    b.Property<int>("RankId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("RankAccessLevel")
                        .HasColumnType("int");

                    b.Property<string>("RankName")
                        .HasColumnType("longtext");

                    b.HasKey("RankId");

                    b.ToTable("Ranks");
                });

            modelBuilder.Entity("GaryPortalAPI.Models.Team", b =>
                {
                    b.Property<int>("TeamId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("TeamAccessLevel")
                        .HasColumnType("int");

                    b.Property<string>("TeamName")
                        .HasColumnType("longtext");

                    b.HasKey("TeamId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("GaryPortalAPI.Models.User", b =>
                {
                    b.Property<string>("UserUUID")
                        .HasColumnType("varchar(255)");

                    b.Property<bool>("IsQueued")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserBio")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UserCreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("UserDateOfBirth")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UserFullName")
                        .HasColumnType("longtext");

                    b.Property<string>("UserGender")
                        .HasColumnType("longtext");

                    b.Property<bool>("UserIsAdmin")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("UserIsStaff")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasColumnType("longtext");

                    b.Property<string>("UserProfileImageUrl")
                        .HasColumnType("longtext");

                    b.Property<string>("UserQuote")
                        .HasColumnType("longtext");

                    b.Property<string>("UserSpanishName")
                        .HasColumnType("longtext");

                    b.Property<string>("UserStanding")
                        .HasColumnType("longtext");

                    b.HasKey("UserUUID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("GaryPortalAPI.Models.UserAuthentication", b =>
                {
                    b.Property<string>("UserUUID")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("UserEmail")
                        .HasColumnType("longtext");

                    b.Property<bool>("UserEmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserPassHash")
                        .HasColumnType("longtext");

                    b.Property<string>("UserPassSalt")
                        .HasColumnType("longtext");

                    b.Property<string>("UserPhone")
                        .HasColumnType("longtext");

                    b.Property<bool>("UserPhoneConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("UserUUID");

                    b.ToTable("UserAuthentications");
                });

            modelBuilder.Entity("GaryPortalAPI.Models.UserPoints", b =>
                {
                    b.Property<string>("UserUUID")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("AmigoPoints")
                        .HasColumnType("int");

                    b.Property<int>("BowelsRelieved")
                        .HasColumnType("int");

                    b.Property<int>("MeaningfulPrayers")
                        .HasColumnType("int");

                    b.Property<int>("PositivityPoints")
                        .HasColumnType("int");

                    b.Property<int>("Prayers")
                        .HasColumnType("int");

                    b.HasKey("UserUUID");

                    b.ToTable("UserPoints");
                });

            modelBuilder.Entity("GaryPortalAPI.Models.UserRanks", b =>
                {
                    b.Property<string>("UserUUID")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("AmigoRankId")
                        .HasColumnType("int");

                    b.Property<int>("PositivtyRankId")
                        .HasColumnType("int");

                    b.HasKey("UserUUID");

                    b.HasIndex("AmigoRankId");

                    b.HasIndex("PositivtyRankId");

                    b.ToTable("UserRanks");
                });

            modelBuilder.Entity("GaryPortalAPI.Models.UserRefreshToken", b =>
                {
                    b.Property<string>("UserUUID")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("TokenClient")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("TokenExpiryDate")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("TokenIsEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime?>("TokenIssueDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("UserUUID", "RefreshToken");

                    b.ToTable("UserRefreshTokens");
                });

            modelBuilder.Entity("GaryPortalAPI.Models.UserTeam", b =>
                {
                    b.Property<string>("UserUUID")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("TeamId")
                        .HasColumnType("int");

                    b.HasKey("UserUUID");

                    b.HasIndex("TeamId");

                    b.ToTable("UserTeams");
                });

            modelBuilder.Entity("GaryPortalAPI.Models.Feed.FeedMediaPost", b =>
                {
                    b.HasBaseType("GaryPortalAPI.Models.Feed.FeedPost");

                    b.Property<string>("PostUrl")
                        .HasColumnType("longtext");

                    b.HasDiscriminator().HasValue("FeedMediaPost");
                });

            modelBuilder.Entity("GaryPortalAPI.Models.Feed.FeedPollPost", b =>
                {
                    b.HasBaseType("GaryPortalAPI.Models.Feed.FeedPost");

                    b.Property<string>("PollQuestion")
                        .HasColumnType("longtext");

                    b.HasDiscriminator().HasValue("FeedPollPost");
                });

            modelBuilder.Entity("GaryPortalAPI.Models.Feed.FeedAnswerVote", b =>
                {
                    b.HasOne("GaryPortalAPI.Models.Feed.FeedPollAnswer", "PollAnswer")
                        .WithMany("Votes")
                        .HasForeignKey("PollAnswerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GaryPortalAPI.Models.User", "User")
                        .WithMany("FeedVotes")
                        .HasForeignKey("UserUUID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PollAnswer");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GaryPortalAPI.Models.Feed.FeedComment", b =>
                {
                    b.HasOne("GaryPortalAPI.Models.Feed.FeedPost", "Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GaryPortalAPI.Models.User", "User")
                        .WithMany("FeedComments")
                        .HasForeignKey("UserUUID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GaryPortalAPI.Models.Feed.FeedLike", b =>
                {
                    b.HasOne("GaryPortalAPI.Models.Feed.FeedPost", "Post")
                        .WithMany("Likes")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GaryPortalAPI.Models.User", "User")
                        .WithMany("FeedLikes")
                        .HasForeignKey("UserUUID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GaryPortalAPI.Models.Feed.FeedPollAnswer", b =>
                {
                    b.HasOne("GaryPortalAPI.Models.Feed.FeedPollPost", "Poll")
                        .WithMany("PollAnswers")
                        .HasForeignKey("PollId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Poll");
                });

            modelBuilder.Entity("GaryPortalAPI.Models.Feed.FeedPost", b =>
                {
                    b.HasOne("GaryPortalAPI.Models.User", "Poster")
                        .WithMany("FeedPosts")
                        .HasForeignKey("PosterUUID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GaryPortalAPI.Models.Team", "PostTeam")
                        .WithMany("FeedPosts")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Poster");

                    b.Navigation("PostTeam");
                });

            modelBuilder.Entity("GaryPortalAPI.Models.UserAuthentication", b =>
                {
                    b.HasOne("GaryPortalAPI.Models.User", "User")
                        .WithOne("UserAuthentication")
                        .HasForeignKey("GaryPortalAPI.Models.UserAuthentication", "UserUUID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("GaryPortalAPI.Models.UserPoints", b =>
                {
                    b.HasOne("GaryPortalAPI.Models.User", "User")
                        .WithOne("UserPoints")
                        .HasForeignKey("GaryPortalAPI.Models.UserPoints", "UserUUID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("GaryPortalAPI.Models.UserRanks", b =>
                {
                    b.HasOne("GaryPortalAPI.Models.Rank", "AmigoRank")
                        .WithMany("UserAmigoRanks")
                        .HasForeignKey("AmigoRankId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GaryPortalAPI.Models.Rank", "PositivityRank")
                        .WithMany("UserPositivityRanks")
                        .HasForeignKey("PositivtyRankId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GaryPortalAPI.Models.User", "User")
                        .WithOne("UserRanks")
                        .HasForeignKey("GaryPortalAPI.Models.UserRanks", "UserUUID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AmigoRank");

                    b.Navigation("PositivityRank");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GaryPortalAPI.Models.UserRefreshToken", b =>
                {
                    b.HasOne("GaryPortalAPI.Models.User", "User")
                        .WithMany("UserRefreshTokens")
                        .HasForeignKey("UserUUID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("GaryPortalAPI.Models.UserTeam", b =>
                {
                    b.HasOne("GaryPortalAPI.Models.Team", "Team")
                        .WithMany("UserTeams")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GaryPortalAPI.Models.User", "User")
                        .WithOne("UserTeam")
                        .HasForeignKey("GaryPortalAPI.Models.UserTeam", "UserUUID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Team");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GaryPortalAPI.Models.Feed.FeedPollAnswer", b =>
                {
                    b.Navigation("Votes");
                });

            modelBuilder.Entity("GaryPortalAPI.Models.Feed.FeedPost", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Likes");
                });

            modelBuilder.Entity("GaryPortalAPI.Models.Rank", b =>
                {
                    b.Navigation("UserAmigoRanks");

                    b.Navigation("UserPositivityRanks");
                });

            modelBuilder.Entity("GaryPortalAPI.Models.Team", b =>
                {
                    b.Navigation("FeedPosts");

                    b.Navigation("UserTeams");
                });

            modelBuilder.Entity("GaryPortalAPI.Models.User", b =>
                {
                    b.Navigation("FeedComments");

                    b.Navigation("FeedLikes");

                    b.Navigation("FeedPosts");

                    b.Navigation("FeedVotes");

                    b.Navigation("UserAuthentication");

                    b.Navigation("UserPoints");

                    b.Navigation("UserRanks");

                    b.Navigation("UserRefreshTokens");

                    b.Navigation("UserTeam");
                });

            modelBuilder.Entity("GaryPortalAPI.Models.Feed.FeedPollPost", b =>
                {
                    b.Navigation("PollAnswers");
                });
#pragma warning restore 612, 618
        }
    }
}
