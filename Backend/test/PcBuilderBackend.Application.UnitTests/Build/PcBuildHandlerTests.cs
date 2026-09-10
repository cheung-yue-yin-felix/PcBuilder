using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using PcBuilderBackend.Application.Build;
using PcBuilderBackend.Application.Build.Commands.CreatePcBuild;
using PcBuilderBackend.Application.Build.Dto;
using PcBuilderBackend.Application.Build.Queries;
using PcBuilderBackend.Application.Common.Authorization;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.UnitTests.Support;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.UnitTests.Build;

public class PcBuildHandlerTests : IDisposable
{
    private readonly AppFixture _fx = new();

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
            _fx.Dispose();
    }

    [Fact]
    public async Task Anonymous_create_persists()
    {
        var pcBuilds = Substitute.For<IPcBuildRepository>();
        var unitOfWork = Substitute.For<IUnitOfWork>();
        var currentUser = Substitute.For<ICurrentUser>();
        currentUser.IsAuthenticated.Returns(false);

        var handler = new CreatePcBuildHandler(
            pcBuilds,
            CompatibleChecker(),
            NullLogger<CreatePcBuildHandler>.Instance,
            currentUser,
            unitOfWork,
            _fx.Mapper);

        var created = await handler.Handle(ValidCreate(), CancellationToken.None);

        created.Should().NotBeNull();
        pcBuilds.Received(1).Add(Arg.Any<PcBuild>());
        pcBuilds.DidNotReceive().AddUser(Arg.Any<PcBuildUser>());
        await unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Member_create_publishes()
    {
        var userId = Guid.NewGuid();
        var pcBuilds = Substitute.For<IPcBuildRepository>();
        var unitOfWork = Substitute.For<IUnitOfWork>();
        var currentUser = Substitute.For<ICurrentUser>();
        currentUser.IsAuthenticated.Returns(true);
        currentUser.IsInRole(AuthRoles.Member).Returns(true);
        currentUser.UserId.Returns(userId);

        var handler = new CreatePcBuildHandler(
            pcBuilds,
            CompatibleChecker(),
            NullLogger<CreatePcBuildHandler>.Instance,
            currentUser,
            unitOfWork,
            _fx.Mapper);

        await handler.Handle(ValidCreate(), CancellationToken.None);

        pcBuilds.Received(1).AddUser(Arg.Is<PcBuildUser>(u => u.UserId == userId && u.IsPublic));
        await unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Admin_create_is_forbidden()
    {
        var pcBuilds = Substitute.For<IPcBuildRepository>();
        var unitOfWork = Substitute.For<IUnitOfWork>();
        var currentUser = Substitute.For<ICurrentUser>();
        currentUser.IsAuthenticated.Returns(true);
        currentUser.IsInRole(AuthRoles.Admin).Returns(true);
        currentUser.IsInRole(AuthRoles.Member).Returns(false);

        var handler = new CreatePcBuildHandler(
            pcBuilds,
            CompatibleChecker(),
            NullLogger<CreatePcBuildHandler>.Instance,
            currentUser,
            unitOfWork,
            _fx.Mapper);

        var act = () => handler.Handle(ValidCreate(), CancellationToken.None);

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
        await unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Get_by_id_forbids_private_build_of_another_user()
    {
        var id = Guid.NewGuid();
        var ownerId = Guid.NewGuid();
        var store = Substitute.For<IPcBuildReadStore>();
        store.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns(new PcBuildDto
        {
            Id = id,
            Name = "Hidden",
            UserId = ownerId,
            IsPublic = false
        });
        var currentUser = Substitute.For<ICurrentUser>();
        currentUser.UserId.Returns(Guid.NewGuid());

        var act = () => new GetPcBuildByIdHandler(store, currentUser)
            .Handle(new GetPcBuildByIdQuery(id), CancellationToken.None);

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task Get_by_id_allows_public_build()
    {
        var id = Guid.NewGuid();
        var dto = new PcBuildDto
        {
            Id = id,
            Name = "Public",
            UserId = Guid.NewGuid(),
            IsPublic = true
        };
        var store = Substitute.For<IPcBuildReadStore>();
        store.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns(dto);
        var currentUser = Substitute.For<ICurrentUser>();
        currentUser.UserId.Returns(Guid.NewGuid());

        var result = await new GetPcBuildByIdHandler(store, currentUser)
            .Handle(new GetPcBuildByIdQuery(id), CancellationToken.None);

        result.Should().BeSameAs(dto);
    }

    private static ICompatibilityChecker CompatibleChecker()
    {
        var checker = Substitute.For<ICompatibilityChecker>();
        checker.CheckCompatibilityAsync(Arg.Any<CompatibilityCheckRequest>())
            .Returns([]);
        return checker;
    }

    private static CreatePcBuildCommand ValidCreate() =>
        new(
            "My build",
            null,
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            null,
            Guid.NewGuid(),
            null,
            Guid.NewGuid(),
            [],
            [],
            [],
            []);
}
