﻿// <auto-generated />
using System;
using LMS.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CompanyWeb.Infrastructure.Migrations
{
    [DbContext(typeof(CompanyDbContext))]
    [Migration("20240829042548_AddColumnRole")]
    partial class AddColumnRole
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CompanyWeb.Domain.Models.Auth.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("text")
                        .HasColumnName("refreshtoken");

                    b.Property<DateTime?>("RefreshTokenExpiredOn")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("refreshtokenexpiredon");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("CompanyWeb.Domain.Models.Entities.Departement", b =>
                {
                    b.Property<int>("Deptno")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("deptno");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Deptno"));

                    b.Property<string>("Deptname")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("deptname");

                    b.Property<int?>("Mgrempno")
                        .HasColumnType("integer")
                        .HasColumnName("mgrempno");

                    b.HasKey("Deptno");

                    b.HasIndex(new[] { "Mgrempno" }, "departements_mgrempno_key")
                        .IsUnique();

                    b.ToTable("departements");
                });

            modelBuilder.Entity("CompanyWeb.Domain.Models.Entities.DepartementLocation", b =>
                {
                    b.Property<int>("LocationId")
                        .HasColumnType("integer")
                        .HasColumnName("locationid");

                    b.Property<int>("Deptno")
                        .HasColumnType("integer")
                        .HasColumnName("deptno");

                    b.HasKey("LocationId", "Deptno");

                    b.HasIndex("Deptno");

                    b.ToTable("departementlocations");
                });

            modelBuilder.Entity("CompanyWeb.Domain.Models.Entities.Employee", b =>
                {
                    b.Property<int>("Empno")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("empno");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Empno"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("address");

                    b.Property<string>("AppUserId")
                        .HasColumnType("text")
                        .HasColumnName("appuserid");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("createdat");

                    b.Property<string>("DeactivateReason")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("deactivatereason");

                    b.Property<int?>("Deptno")
                        .HasColumnType("integer")
                        .HasColumnName("deptno");

                    b.Property<int?>("DirectSupervisor")
                        .HasColumnType("integer")
                        .HasColumnName("directsupervisor");

                    b.Property<DateOnly>("Dob")
                        .HasColumnType("date")
                        .HasColumnName("dob");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("email");

                    b.Property<int>("EmpLevel")
                        .HasColumnType("integer")
                        .HasColumnName("emplevel");

                    b.Property<string>("EmpType")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("emptype");

                    b.Property<string>("Fname")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("fname");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean")
                        .HasColumnName("isactive");

                    b.Property<string>("Lname")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("lname");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)")
                        .HasColumnName("phonenumber");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("position");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("role");

                    b.Property<int>("Salary")
                        .HasColumnType("integer")
                        .HasColumnName("salary");

                    b.Property<string>("Sex")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("sex");

                    b.Property<string>("Ssn")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)")
                        .HasColumnName("ssn");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updatedat");

                    b.HasKey("Empno");

                    b.HasIndex("Deptno");

                    b.ToTable("employees");
                });

            modelBuilder.Entity("CompanyWeb.Domain.Models.Entities.EmployeeDependent", b =>
                {
                    b.Property<int>("EmpDependentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("empdependentid");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("EmpDependentId"));

                    b.Property<DateOnly?>("BirthDate")
                        .HasColumnType("date")
                        .HasColumnName("birthdate");

                    b.Property<int>("DependentEmpno")
                        .HasColumnType("integer")
                        .HasColumnName("dependentempno");

                    b.Property<string>("Fname")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("fname");

                    b.Property<string>("Lname")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("lname");

                    b.Property<string>("Relation")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("relation");

                    b.Property<string>("Sex")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("sex");

                    b.HasKey("EmpDependentId");

                    b.ToTable("employeedependents");
                });

            modelBuilder.Entity("CompanyWeb.Domain.Models.Entities.Location", b =>
                {
                    b.Property<int>("LocationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("locationid");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("LocationId"));

                    b.Property<string>("LocationName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("locationname");

                    b.HasKey("LocationId");

                    b.ToTable("locations");
                });

            modelBuilder.Entity("CompanyWeb.Domain.Models.Entities.Project", b =>
                {
                    b.Property<int>("Projno")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("projno");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Projno"));

                    b.Property<int>("Deptno")
                        .HasColumnType("integer")
                        .HasColumnName("deptno");

                    b.Property<int>("ProjLocation")
                        .HasColumnType("integer")
                        .HasColumnName("projlocation");

                    b.Property<string>("Projname")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("projname");

                    b.HasKey("Projno");

                    b.HasIndex("Deptno");

                    b.ToTable("projects");
                });

            modelBuilder.Entity("CompanyWeb.Domain.Models.Entities.Workson", b =>
                {
                    b.Property<int>("Empno")
                        .HasColumnType("integer")
                        .HasColumnName("empno");

                    b.Property<int>("Projno")
                        .HasColumnType("integer")
                        .HasColumnName("projno");

                    b.Property<DateOnly>("Dateworked")
                        .HasColumnType("date")
                        .HasColumnName("dateworked");

                    b.Property<int>("Hoursworked")
                        .HasColumnType("integer")
                        .HasColumnName("hoursworked");

                    b.HasKey("Empno", "Projno");

                    b.HasIndex("Projno");

                    b.ToTable("worksons");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("CompanyWeb.Domain.Models.Entities.Departement", b =>
                {
                    b.HasOne("CompanyWeb.Domain.Models.Entities.Employee", "MgrempnoNavigation")
                        .WithOne("Departement")
                        .HasForeignKey("CompanyWeb.Domain.Models.Entities.Departement", "Mgrempno");

                    b.Navigation("MgrempnoNavigation");
                });

            modelBuilder.Entity("CompanyWeb.Domain.Models.Entities.DepartementLocation", b =>
                {
                    b.HasOne("CompanyWeb.Domain.Models.Entities.Departement", "DeptnoNavigation")
                        .WithMany("DepartementLocation")
                        .HasForeignKey("Deptno")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CompanyWeb.Domain.Models.Entities.Location", "LocationIdNavigation")
                        .WithMany("DepartementLocation")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DeptnoNavigation");

                    b.Navigation("LocationIdNavigation");
                });

            modelBuilder.Entity("CompanyWeb.Domain.Models.Entities.Employee", b =>
                {
                    b.HasOne("CompanyWeb.Domain.Models.Entities.Departement", "DeptnoNavigation")
                        .WithMany("Employees")
                        .HasForeignKey("Deptno");

                    b.Navigation("DeptnoNavigation");
                });

            modelBuilder.Entity("CompanyWeb.Domain.Models.Entities.Project", b =>
                {
                    b.HasOne("CompanyWeb.Domain.Models.Entities.Departement", "DeptnoNavigation")
                        .WithMany("Projects")
                        .HasForeignKey("Deptno")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DeptnoNavigation");
                });

            modelBuilder.Entity("CompanyWeb.Domain.Models.Entities.Workson", b =>
                {
                    b.HasOne("CompanyWeb.Domain.Models.Entities.Employee", "EmpnoNavigation")
                        .WithMany("Worksons")
                        .HasForeignKey("Empno")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CompanyWeb.Domain.Models.Entities.Project", "ProjnoNavigation")
                        .WithMany("Worksons")
                        .HasForeignKey("Projno")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EmpnoNavigation");

                    b.Navigation("ProjnoNavigation");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("CompanyWeb.Domain.Models.Auth.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("CompanyWeb.Domain.Models.Auth.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CompanyWeb.Domain.Models.Auth.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("CompanyWeb.Domain.Models.Auth.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CompanyWeb.Domain.Models.Entities.Departement", b =>
                {
                    b.Navigation("DepartementLocation");

                    b.Navigation("Employees");

                    b.Navigation("Projects");
                });

            modelBuilder.Entity("CompanyWeb.Domain.Models.Entities.Employee", b =>
                {
                    b.Navigation("Departement");

                    b.Navigation("Worksons");
                });

            modelBuilder.Entity("CompanyWeb.Domain.Models.Entities.Location", b =>
                {
                    b.Navigation("DepartementLocation");
                });

            modelBuilder.Entity("CompanyWeb.Domain.Models.Entities.Project", b =>
                {
                    b.Navigation("Worksons");
                });
#pragma warning restore 612, 618
        }
    }
}
