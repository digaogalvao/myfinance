import { useEffect, useState } from 'react';
import { Api } from '../providers/Api';
import { DateTimePicker } from '../components/DateTimePicker/DateTimePicker';
import { Navmenu } from '../components/Nav/nav';
import { Col, Row, Table, Form, Button } from 'react-bootstrap';
import { format } from 'date-fns';

export function FluxoCaixaPage() {
  // Tipos para o estado
  interface IFluxoCaixa {
    totalGastos: number;
    totalRecebidos: number;
    saldoFinal: number;
    dataInicial: string;
    dataFinal: string;
  }

  const [fluxoCaixa, setFluxoCaixa] = useState<IFluxoCaixa | null>(null);
  const [dataInicial, setDataInicial] = useState<Date | null>(new Date());
  const [dataFinal, setDataFinal] = useState<Date | null>(new Date());
  const [mes, setMes] = useState<number | null>(null);
  const [ano, setAno] = useState<number | null>(null);
  const [filtroPorPeriodo, setFiltroPorPeriodo] = useState<boolean>(false);

  // Recupera o usuário logado (ajuste conforme sua aplicação)
  const usuarioId = Number(localStorage.getItem('userId'));

  // Formatação de data
  const formatDate = (date: Date | null) => (date ? format(date, 'yyyy-MM-dd') : '');

  // Função para buscar os lançamentos de fluxo de caixa
  const fetchFluxoCaixa = async () => {
    try {
      const params = {
        usuarioId, // Adiciona o usuário na requisição
        ...(filtroPorPeriodo
          ? {
              dataInicial: formatDate(dataInicial),
              dataFinal: formatDate(dataFinal),
            }
          : {
              mes,
              ano,
            }),
      };

      const response = await Api.get('Lancamentos/fluxo-caixa', { params });
      //const result = response.data?.result;
      const result = response.data;

      if (result) {
        setFluxoCaixa({
          totalGastos: result.totalGastos,
          totalRecebidos: result.totalRecebidos,
          saldoFinal: result.saldoFinal,
          dataInicial: result.dataInicial,
          dataFinal: result.dataFinal,
        });
      } else {
        setFluxoCaixa(null);
      }
    } catch (error) {
      console.error('Erro ao buscar fluxo de caixa:', error);
      setFluxoCaixa(null);
    }
  };

  // Efeito para buscar os dados quando filtros mudam
  useEffect(() => {
    if (usuarioId) {
      fetchFluxoCaixa();
    }
  }, [dataInicial, dataFinal, mes, ano, filtroPorPeriodo, usuarioId]);

  return (
    <>
      <Navmenu />
      <div className="container">
        <h3>Relatório de Fluxo de Caixa</h3>
        <Form>
          <Row className="mb-3">
            <Col xs="auto">
              <Form.Check
                type="radio"
                label="Filtrar por Período"
                checked={filtroPorPeriodo}
                onChange={() => setFiltroPorPeriodo(true)}
              />
            </Col>
            <Col xs="auto">
              <Form.Check
                type="radio"
                label="Filtrar por Mês e Ano"
                checked={!filtroPorPeriodo}
                onChange={() => setFiltroPorPeriodo(false)}
              />
            </Col>
          </Row>

          {filtroPorPeriodo ? (
            <Row className="mb-3">
              <Col sm={3}>
                <Form.Label>Data Inicial</Form.Label>
                <DateTimePicker selectedData={dataInicial} handlerData={setDataInicial} />
              </Col>
              <Col sm={3}>
                <Form.Label>Data Final</Form.Label>
                <DateTimePicker selectedData={dataFinal} handlerData={setDataFinal} />
              </Col>
            </Row>
          ) : (
            <Row className="mb-3">
              <Col sm={3}>
                <Form.Label>Mês</Form.Label>
                <Form.Control
                  type="number"
                  min="1"
                  max="12"
                  value={mes ?? ''}
                  onChange={(e) => setMes(Number(e.target.value))}
                />
              </Col>
              <Col sm={3}>
                <Form.Label>Ano</Form.Label>
                <Form.Control
                  type="number"
                  min="2000"
                  value={ano ?? ''}
                  onChange={(e) => setAno(Number(e.target.value))}
                />
              </Col>
            </Row>
          )}

          <Button variant="success" onClick={fetchFluxoCaixa}>
            Visualizar
          </Button>
        </Form>

        {/* Tabela de Resultados */}
        <Table striped bordered hover className="mt-3">
          <thead>
            <tr>
              <th>Data Inicial</th>
              <th>Data Final</th>
              <th>Total Gastos</th>
              <th>Total Recebidos</th>
              <th>Saldo Final</th>
            </tr>
          </thead>
          <tbody>
            {fluxoCaixa ? (
              <tr>
                <td>{format(new Date(fluxoCaixa.dataInicial), 'dd/MM/yyyy')}</td>
                <td>{format(new Date(fluxoCaixa.dataFinal), 'dd/MM/yyyy')}</td>
                <td className={fluxoCaixa.totalGastos > 0 ? 'text-danger' : ''}>
                  {`R$ -${fluxoCaixa.totalGastos.toFixed(2)}`}
                </td>
                <td className={fluxoCaixa.totalRecebidos > 0 ? 'text-success' : ''}>
                  {`R$ ${fluxoCaixa.totalRecebidos.toFixed(2)}`}
                </td>
                <td className={fluxoCaixa.saldoFinal < 0 ? 'text-danger' : 'text-success'}>
                  {`R$ ${fluxoCaixa.saldoFinal.toFixed(2)}`}
                </td>
              </tr>
            ) : (
              <tr>
                <td colSpan={5}>Nenhum resultado encontrado.</td>
              </tr>
            )}
          </tbody>
        </Table>
      </div>
    </>
  );
}
