import axios from 'axios';

const baseURL = process.env.REACT_APP_BASE_URL;

// Criação da instância do Axios
const Api = axios.create({
    baseURL,
    headers: {
      'Content-Type': 'application/json',
    },
});

// Interceptor para adicionar o token Bearer em todas as requisições
Api.interceptors.request.use(
    (config) => {
      const token = localStorage.getItem('token');  // Obtém o token do localStorage
      if (token) {
        config.headers['Authorization'] = `Bearer ${token}`;  // Adiciona o token no header
      }
      return config;  // Retorna a configuração da requisição com o header atualizado
    },
    (error) => {
      return Promise.reject(error);  // Caso haja erro, retorna a promessa rejeitada
    }
);
  
export { Api };
