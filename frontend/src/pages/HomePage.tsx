import { Navmenu } from '../components/Nav/nav';
import background from '../assets/background.jpg';
import { useAuth } from '../providers/AuthContext';
import { Navigate } from 'react-router-dom';

export function HomePage() {
  const { isAuthenticated } = useAuth();
  
  if (!isAuthenticated) {
    return <Navigate to="/login" replace />;
  }
  
  const backgroundStyle = {
    backgroundImage: `url(${background})`,
    backgroundSize: 'cover',
    backgroundRepeat: 'no-repeat',
    backgroundPosition: 'center',
    height: '100vh',
  };

  return (
    <>
    <div style={backgroundStyle}>
      <Navmenu />
    </div>
    </>
  );
}
