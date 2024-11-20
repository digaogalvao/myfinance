import React, { createContext, useContext, useState, ReactNode, useEffect } from 'react';
import { Api } from './Api';

// Definindo os tipos
interface AuthContextType {
  isAuthenticated: boolean;
  login: (email: string, senha: string) => void;
  logout: () => void;
  userId: string | null;
  userName: string | null;
  token: string | null;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return context;
};

interface AuthProviderProps {
  children: ReactNode;
}

export const AuthProvider = ({ children }: AuthProviderProps) => {
  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);
  const [userId, setUserId] = useState<string | null>(localStorage.getItem('userId'));
  const [userName, setUserName] = useState<string | null>(localStorage.getItem('userName'));
  const [token, setToken] = useState<string | null>(localStorage.getItem('token'));
  const [isLoading, setIsLoading] = useState<boolean>(true);

  const logout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('userId');
    localStorage.removeItem('userName');
    setToken(null);
    setUserId(null);
    setUserName(null);
    setIsAuthenticated(false);
  };
  
  // Use effect to check token in localStorage
  useEffect(() => {
    const storedToken = localStorage.getItem('token');
    const storedUserId = localStorage.getItem('userId');
    const storedUserName = localStorage.getItem('userName');

    if (storedToken && storedUserId && storedUserName) {
      try {
        const decodedToken = JSON.parse(atob(storedToken.split('.')[1]));

        if (decodedToken && decodedToken.exp * 1000 > Date.now()) {
          setToken(storedToken);
          setUserId(storedUserId);
          setUserName(storedUserName);
          setIsAuthenticated(true);
        } else {
          logout(); // Se o token for inválido ou expirado, faça logout
        }
      } catch (error) {
        console.error('Erro ao decodificar o token:', error);
        logout();
      }
    } else {
      setIsAuthenticated(false); // Se não houver token, a autenticação é inválida
    }
    setIsLoading(false); // Atualiza quando a verificação de autenticação for concluída
  }, []);

  if (isLoading) {
    return <div>Loading...</div>; // Mostra um carregamento até a verificação ser feita
  }

  const login = async (email: string, senha: string) => {
    try {
      // Fazendo a requisição POST para a API usando a instância axios
      const response = await Api.post('usuarios/login', {
        email,
        senha,
      });
  
      if (response.status === 200) {
        const Token = response.data.token; // Supondo que o token seja retornado como parte do response.data
        const UserName = response.data.userName;
        const UserId = response.data.userId;

        // Guardando o token no localStorage
        localStorage.setItem('token', Token);
        localStorage.setItem('userId', UserId);
        localStorage.setItem('userName', UserName);
        
        setToken(Token);  // Atualiza o token no estado
        setUserId(UserId);  // Atualiza o id usuário no estado
        setUserName(UserName);  // Atualiza o id usuário no estado
        setIsAuthenticated(true);  // Define o usuário como autenticado
      } else {
        alert('Login falhou. Verifique suas credenciais.');
      }
    } catch (error) {
      console.error('Erro ao fazer login:', error);
      alert('Erro ao conectar ao servidor. Tente novamente mais tarde.');
    }
  };

  return (
    <AuthContext.Provider value={{ isAuthenticated, login, logout, userId, userName, token }}>
      {children}
    </AuthContext.Provider>
  );
};
