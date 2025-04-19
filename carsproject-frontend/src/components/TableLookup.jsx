import React from 'react';
import './TableLookup.css';

const TableLookup = ({ columns, data}) => {
  console.log('Columns:', columns);
  console.log('Data:', data);  

  return (

    <div className="table-container">
      
      <table className="custom-table">       
          
          <thead>
            <tr>
              {columns.map((col) => (
                <th key={col.accessor}>{col.header}</th>
              ))}
            </tr>
          </thead>
          <tbody>
            {data.map((item, index) => (
              <tr key={index} className={index % 2 === 0 ? 'row-light' : 'row-white'}>
                {columns.map(col => (
                  <td key={col.accessor}>{item[col.accessor]}</td>
                ))}
              </tr>
            ))}
          </tbody>              
      </table>
    </div>
  );
};

export default TableLookup;
