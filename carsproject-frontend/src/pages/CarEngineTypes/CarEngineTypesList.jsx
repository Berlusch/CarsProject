import { observer } from "mobx-react-lite";
import { useEffect } from "react";
import CarEngineTypeStore from "../../stores/CarEngineTypeStore";
import TableLookup from "../../components/TableLookup";
import Pagination from "../../components/Pagination";

const CarEngineTypeList = observer(() => {
  useEffect(() => {
    CarEngineTypeStore.fetchCarEngineTypes();
  }, []);

  const handlePageChange = (page) => {
    CarEngineTypeStore.setPage(page);
  };

  const columns = [
    { header: "Type", accessor: "type" },
    { header: "Abbreviation", accessor: "abrv" }
  ];

  const data = CarEngineTypeStore.carEngineTypes.map((item) => ({
    type: item.type,
    abrv: item.abrv
  }));

  if (CarEngineTypeStore.loading) return <p>Loading...</p>;
  if (CarEngineTypeStore.error) return <p>{CarEngineTypeStore.error}</p>;

  return (
    <div>
      <header className="entityName">Car Engine Types</header>
      <br/>

      <TableLookup columns={columns} data={data} />

      <Pagination
        currentPage={CarEngineTypeStore.currentPage}
        onPageChange={handlePageChange}
        hasNextPage={CarEngineTypeStore.hasNextPage}
      />
    </div>
  );
});

export default CarEngineTypeList;
