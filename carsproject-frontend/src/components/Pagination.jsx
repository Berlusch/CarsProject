import React from 'react';
import './Pagination.css'; 

const Pagination = ({ currentPage, onPageChange, hasNextPage, currentPageSize }) => {
    const handlePrevPage = () => {
      if (currentPage > 1) {
        onPageChange(currentPage - 1);
      }
    };
  
    const handleNextPage = () => {
      if (hasNextPage) {
        onPageChange(currentPage + 1);        
      }
    };
  
    return (
      <div className="pagination">
        <button
          className="pagination-btn"
          onClick={handlePrevPage}
          disabled={currentPage === 1}
        >
          Prev
        </button>
        <span className="pagination-info">
          Page {currentPage}
        </span>
        <button
          className="pagination-btn"
          onClick={handleNextPage}
          disabled={!hasNextPage} 
        >
          Next
        </button>
      </div>
    );
  };
  

export default Pagination;
