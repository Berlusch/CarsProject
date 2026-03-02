using CarsProject.Common;
using CarsProject.Model;
using CarsProject.Repository.Common;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System.Linq.Expressions;


namespace CarsProject.Service.Tests;

public class CarModelServiceTests
{
    private readonly Mock<IGenericRepository<CarModel>> _repoMock;
    private readonly CarModelService _service;

    public CarModelServiceTests()
    {
        _repoMock = new Mock<IGenericRepository<CarModel>>();
        _service = new CarModelService(_repoMock.Object);
    }
   

    private IQueryable<CarModel> BuildQueryable(params CarModel[] items)
        => new TestAsyncEnumerable<CarModel>(items);

    

    private CarModel CreateModel(
        int id,
        string name = "TestModel",
        string abrv = "TM",
        int makeId = 1,
        int engineId = 1)
    {
        return new CarModel
        {
            Id = id,
            Name = name,
            Abrv = abrv,
            CarMakeId = makeId,
            CarEngineTypeId = engineId,

            CarMake = new CarMake { Id = makeId, Name = "VW", Abrv = "VW" },
            CarEngineType = new CarEngineType { Id = engineId, Type = "Diesel" }
        };
    }
       

    [Fact]
    public async Task GetCarModelsAsync_ReturnsPagedResult()
    {
        var data = BuildQueryable(
            CreateModel(1, "A"),
            CreateModel(2, "B"),
            CreateModel(3, "C")
        );

        _repoMock.Setup(r => r.GetQuery(It.IsAny<PFSParameters>()))
                 .Returns(data);

        var pfs = new PFSParameters
        {
            Paging = new PagingParameters { PageNumber = 1, PageSize = 2 }
        };

        var result = await _service.GetCarModelsAsync(pfs);

        Assert.Equal(3, result.TotalCount);
        Assert.Equal(2, result.Items.Count);
        Assert.Equal("A", result.Items[0].Name);
        Assert.Equal("B", result.Items[1].Name);
    }          
       
    

    [Fact]
    public async Task AddCarModelAsync_Throws_WhenNameExists()
    {
        var existing = CreateModel(1, "Golf");

        _repoMock.Setup(r => r.GetQuery(It.Is<PFSParameters>(p =>
            p.Filter.PropertyName == "Name")))
            .Returns(BuildQueryable(existing));

        await Assert.ThrowsAsync<Exception>(() =>
            _service.AddCarModelAsync(CreateModel(0, "Golf")));
    }
        
    

    [Fact]
    public async Task UpdateCarModelAsync_Throws_WhenNotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(999))
                 .ThrowsAsync(new KeyNotFoundException("Not found"));

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.UpdateCarModelAsync(999, CreateModel(0)));
    }
    
    [Fact]
    public async Task DeleteCarModelAsync_ReturnsTrue()
    {
        _repoMock.Setup(r => r.DeleteAsync(1))
                 .ReturnsAsync(true);

        var result = await _service.DeleteCarModelAsync(1);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteCarModelAsync_ReturnsFalse()
    {
        _repoMock.Setup(r => r.DeleteAsync(1))
                 .ReturnsAsync(false);

        var result = await _service.DeleteCarModelAsync(1);

        Assert.False(result);
    }
}


public class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider { private readonly IQueryProvider _inner; public TestAsyncQueryProvider(IQueryProvider inner) { _inner = inner; } public IQueryable CreateQuery(Expression expression) => new TestAsyncEnumerable<TEntity>(expression); public IQueryable<TElement> CreateQuery<TElement>(Expression expression) => new TestAsyncEnumerable<TElement>(expression); public object Execute(Expression expression) => _inner.Execute(expression); public TResult Execute<TResult>(Expression expression) => _inner.Execute<TResult>(expression); public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default) => _inner.Execute<TResult>(expression); }
public class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T> { public TestAsyncEnumerable(IEnumerable<T> enumerable) : base(enumerable) { } public TestAsyncEnumerable(Expression expression) : base(expression) { } public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default) => new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator()); IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider<T>(this); }
public class TestAsyncEnumerator<T> : IAsyncEnumerator<T> { private readonly IEnumerator<T> _inner; public TestAsyncEnumerator(IEnumerator<T> inner) { _inner = inner; } public T Current => _inner.Current; public ValueTask DisposeAsync() { _inner.Dispose(); return ValueTask.CompletedTask; } public ValueTask<bool> MoveNextAsync() => new ValueTask<bool>(_inner.MoveNext()); }