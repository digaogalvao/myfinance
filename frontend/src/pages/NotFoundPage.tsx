import { Navmenu } from '../components/Nav/nav';

export function NotFoundPage() {

  return (
    <>
    <Navmenu />
    <div className="container">
      <h2>Ops!</h2>
      <h3>Página não encontrada.</h3>
      <p>O endereço informado não existe na aplicação</p>
    </div>
    </>
  );
}
