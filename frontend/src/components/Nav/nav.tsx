import { Container, Nav, Navbar, NavDropdown } from 'react-bootstrap';
import { Link, useNavigate } from 'react-router-dom';  // Importando o Link do react-router-dom
import { useAuth } from '../../providers/AuthContext';

export function Navmenu() {
  const { userName, logout } = useAuth(); // Obtendo o usuário e a função de logout
  const navigate = useNavigate(); // Usando o hook useNavigate

  const handleLogout = () => {
    logout(); // Realizando o logout
    navigate('/login'); // Redirecionando para a página de login
  };

  return (
    <Navbar className="navbar navbar-dark bg-primary py-4" expand="lg">
      <Container>
        <Navbar.Brand>
          <svg xmlns="http://www.w3.org/2000/svg" id="Layer_1" data-name="Layer 1" viewBox="0 0 24 24" width="40px" height="40px">
            <path d="M12,0C5.383,0,0,5.383,0,12s5.383,12,12,12,12-5.383,12-12S18.617,0,12,0Zm0,22c-5.514,0-10-4.486-10-10S6.486,2,12,2s10,4.486,10,10-4.486,10-10,10Zm4-8c0,1.654-1.346,3-3,3v1c0,.553-.447,1-1,1s-1-.447-1-1v-1h-.268c-1.067,0-2.063-.574-2.598-1.499-.277-.479-.113-1.09,.364-1.366,.479-.279,1.091-.113,1.366,.364,.179,.31,.511,.501,.867,.501h2.268c.552,0,1-.448,1-1,0-.378-.271-.698-.644-.76l-3.041-.507c-1.342-.223-2.315-1.373-2.315-2.733,0-1.654,1.346-3,3-3v-1c0-.552,.447-1,1-1s1,.448,1,1v1h.268c1.067,0,2.063,.575,2.598,1.5,.277,.478,.113,1.089-.364,1.366-.48,.277-1.091,.113-1.366-.365-.179-.309-.511-.5-.867-.5h-2.268c-.552,0-1,.449-1,1,0,.378,.271,.698,.644,.76l3.041,.507c1.342,.223,2.315,1.373,2.315,2.733Z"/>
          </svg>
        </Navbar.Brand>
        <Navbar.Brand as={Link} to="/" title="Financial Management">Controle Financeiro</Navbar.Brand>  {/* Usando Link para navegação interna */}
        <Navbar.Toggle aria-controls="basic-navbar-nav" />
        <Navbar.Collapse id="basic-navbar-nav">
          <Nav className="me-auto">
            <Nav.Link as={Link} to="/lancamento">Lançamentos</Nav.Link>  {/* Usando Link aqui também */}
            <Nav.Link as={Link} to="/fluxocaixa">Fluxo Caixa</Nav.Link>  {/* Usando Link */}
          </Nav>
          <Nav>
            {/* <Nav.Link onClick={handleLogout}>Sair</Nav.Link> */}
            <NavDropdown title={userName} id="basic-nav-dropdown">
              <NavDropdown.Item as={Link} to="/">Sobre</NavDropdown.Item>
              <NavDropdown.Divider />
              <NavDropdown.Item onClick={handleLogout}>Sair</NavDropdown.Item>
            </NavDropdown>
          </Nav>
        </Navbar.Collapse>
      </Container>
    </Navbar>
  );
}
