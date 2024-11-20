import { Routes, Route, Navigate } from 'react-router-dom';
import { HomePage } from '../pages/HomePage';
import { LoginPage } from '../pages/LoginPage';
import { FluxoCaixaPage } from '../pages/FluxoCaixaPage';
import { NotFoundPage } from '../pages/NotFoundPage';
import { LancamentoPage } from '../pages/LancamentoPage';
import { PrivateRoute } from './PrivateRoute';

export function AppRoutes() {
  return (
    <Routes>
      <Route path="/login" element={<LoginPage />} />
      <Route
        path="/"
        element={
          <PrivateRoute element={<HomePage />} path="/" />
        }
      />
      <Route
        path="/fluxocaixa"
        element={
          <PrivateRoute element={<FluxoCaixaPage />} path="/fluxocaixa" />
        }
      />
      <Route
        path="/lancamento"
        element={
          <PrivateRoute element={<LancamentoPage />} path="/lancamento" />
        }
      />
      <Route path="*" element={<NotFoundPage />} />
    </Routes>
  );
}
