import { useEffect } from "react";
import { observer } from "mobx-react-lite";
import CarEngineTypeStore from "../../stores/CarEngineTypeStore";
import TableLookup from "../../components/TableLookup";  

const CarEngineTypeList = observer(() => {
  useEffect(() => {
    CarEngineTypeStore.fetchCarEngineTypes();
  }, []);

  const { carEngineTypes, loading, error } = CarEngineTypeStore;

  const columns = [
    { accessor: 'type', header: 'Type' },
    { accessor: 'abrv', header: 'Abbreviation' }
  ];
 
  const data = carEngineTypes ? carEngineTypes.map((item) => ({
    type: item.type,
    abrv: item.abrv
  })) : [];

  if (loading) return <p>Loading...</p>;
  if (error) return <p>{error}</p>;

  return (
    <div>
      <header className="entityName">
        Car Engine Types
      </header> 
      <br/><br/>

    <TableLookup
     columns={columns}
     data={data} />
    </div>
  );
});

export default CarEngineTypeList;
