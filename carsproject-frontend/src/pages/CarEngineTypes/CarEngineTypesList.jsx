import { observer } from "mobx-react-lite";
import { useEffect, useState } from "react";
import CarEngineTypeStore from "../../stores/CarEngineTypeStore";
import Pagination from "../../components/Pagination";

const CarEngineTypeList = observer(() => {
  const [sortConfig, setSortConfig] = useState({
    key: "type",       
    direction: "desc", 
  });

  useEffect(() => {
    CarEngineTypeStore.fetchCarEngineTypes();
  }, []);

  const handlePageChange = (page) => {
    CarEngineTypeStore.setPage(page);
  };

  const handleSort = (key) => {
    let direction = "asc";
    if (sortConfig.key === key && sortConfig.direction === "asc") {
      direction = "desc";
    }
    setSortConfig({ key, direction });
  };

  const columns = [
    { header: "Type", accessor: "type" },
    { header: "Abbreviation", accessor: "abrv" }
  ];

  let data = CarEngineTypeStore.carEngineTypes.map((item) => ({
    type: item.type,
    abrv: item.abrv
  }));

  
  data = [...data].sort((a, b) => {
    if (a[sortConfig.key] < b[sortConfig.key]) return sortConfig.direction === "asc" ? -1 : 1;
    if (a[sortConfig.key] > b[sortConfig.key]) return sortConfig.direction === "asc" ? 1 : -1;
    return 0;
  });

  if (CarEngineTypeStore.loading) return <p>Loading...</p>;
  if (CarEngineTypeStore.error) return <p>{CarEngineTypeStore.error}</p>;

  return (
    <div>
      <header className="entityName">Car Engine Types</header>
      <br/>

      <div className="table-container">
        <table className="custom-table">
          <thead>
            <tr>
              {columns.map((col) => (
                <th
                  key={col.accessor}
                  onClick={() => handleSort(col.accessor)}
                  style={{ cursor: "pointer" }}
                >
                  {col.header}{" "}
                  {sortConfig.key === col.accessor && (
                    <span>{sortConfig.direction === "asc" ? "↑" : "↓"}</span>
                  )}
                </th>
              ))}
            </tr>
          </thead>
          <tbody>
            {data.map((item, index) => (
              <tr key={index} className={index % 2 === 0 ? "row-light" : "row-white"}>
                {columns.map((col) => (
                  <td key={col.accessor}>{item[col.accessor]}</td>
                ))}
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      <Pagination
        currentPage={CarEngineTypeStore.currentPage}
        onPageChange={handlePageChange}
        hasNextPage={CarEngineTypeStore.hasNextPage}
      />
    </div>
  );
});

export default CarEngineTypeList;