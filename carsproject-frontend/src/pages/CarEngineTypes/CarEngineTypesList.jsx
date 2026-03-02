import { observer } from "mobx-react-lite";
import { useEffect, useState } from "react";
import CarEngineTypeStore from "../../stores/CarEngineTypeStore";
import Pagination from "../../components/Pagination";

const CarEngineTypeList = observer(() => {
  const [sortConfig, setSortConfig] = useState({
    key: "type",
    direction: "asc",
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

  const data = [...CarEngineTypeStore.carEngineTypes].sort((a, b) => {
    const valA = a[sortConfig.key]?.toLowerCase() ?? "";
    const valB = b[sortConfig.key]?.toLowerCase() ?? "";
    return sortConfig.direction === "asc" ? valA.localeCompare(valB) : valB.localeCompare(valA);
  });

  if (CarEngineTypeStore.loading) return <p>Loading...</p>;
  if (CarEngineTypeStore.error) return <p>{CarEngineTypeStore.error}</p>;

  return (
    <div>
      <header className="entityName">Car Engine Types</header>
      <br />

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