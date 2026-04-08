import { useState, useEffect } from 'react';
import './GlobalToast.css';

export function GlobalToast() {
  const [toast, setToast] = useState({ show: false, message: '' });

  useEffect(() => {
    const handleGlobalError = (event) => {
      setToast({ show: true, message: event.detail.message });
      
      // Auto-hide after 5 seconds
      setTimeout(() => {
        setToast({ show: false, message: '' });
      }, 5000);
    };

    window.addEventListener('global-api-error', handleGlobalError);

    return () => {
      window.removeEventListener('global-api-error', handleGlobalError);
    };
  }, []);

  if (!toast.show) return null;

  return (
    <div className="global-toast">
      <div className="global-toast__content">
        <span className="global-toast__icon">⚠️</span>
        <p className="global-toast__message">{toast.message}</p>
        <button 
          className="global-toast__close" 
          onClick={() => setToast({ show: false, message: '' })}
        >
          &times;
        </button>
      </div>
    </div>
  );
}
