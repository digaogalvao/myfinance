import { Navmenu } from '../components/Nav/nav';
import { Api } from '../providers/Api';
import { SetStateAction, useEffect, useState } from 'react';
import { Button, Col, Row, Table } from 'react-bootstrap';
import {Modal, ModalBody, ModalFooter, ModalHeader} from 'reactstrap';
import { format } from 'date-fns';
import { SelectTipo } from '../components/SelectTipo/SelectTipo';
import { SelectNatureza } from '../components/SelectNatureza/SelectNatureza';
import { DateTimePicker } from '../components/DateTimePicker/DateTimePicker';
import { Pagination } from '../components/Pagination/Pagination';
export function LancamentoPage() {
  interface ILancamento {
    id?: any;
    id_Usuario?: any;
    natureza?: any;
    tipo?: any;
    descricao: string;
    valor: any;
    data_Pagamento?: Date;
    data_Cadastro?: Date;
    observacao?: any;
  }

  const [data, setData] = useState<ILancamento[]>([]);

  //Modals
  const [showInsert, setShowInsert] = useState(false);
  const [showEdit, setShowEdit] = useState(false);
  const [showDelete, setShowDelete] = useState(false);
  const [currentPage, setCurrentPage] = useState(1);
  const [dataPerPage] = useState(5);

  // Recupera o usuário logado (ajuste conforme sua aplicação)
  const usuarioId = Number(localStorage.getItem('userId'));

  const [lancamentoSelecionado, setLancamentoSelecionado] = useState<ILancamento>({
    id: undefined,
    id_Usuario: usuarioId,
    natureza: undefined,
    tipo: undefined,
    descricao: '',
    valor: undefined,
    data_Pagamento: undefined,
    data_Cadastro: new Date(),
    observacao: undefined,
  })

  const handleChange = (e: { target: { name: any; value: any; }; }) => {
    const { name, value } = e.target;
  
    if (name === 'data') {
      // Validar se o valor inserido é uma data válida antes de tentar convertê-lo
      const isValidDate = !isNaN(new Date(value).getTime());
  
      setLancamentoSelecionado({
        ...lancamentoSelecionado,
        [name]: isValidDate ? new Date(value) : undefined,
      });
    } else {
      setLancamentoSelecionado({
        ...lancamentoSelecionado,
        [name]: value,
      });
    }
  };

  // Get current posts
  const indexOfLastData = currentPage * dataPerPage;
  const indexOfFirstData = indexOfLastData - dataPerPage;
  const currentData = data.slice(indexOfFirstData, indexOfLastData);

  // Change page
  const paginate = (pageNumber: SetStateAction<number>) => setCurrentPage(pageNumber);

  //Modal controle do estado
  const showHideInsert = () => {
    setShowInsert(!showInsert);
  }

  const showHideEdit = () => {
    setShowEdit(!showEdit);
  }

    const showHideDelete = () => {
    setShowDelete(!showDelete);
  }

  //Lista lançamentos
  const lancamentoGet=async()=>{
    await Api.get(`lancamentos/user/${usuarioId}`)
    .then(response=>{
      setData(response.data);
      //console.log(response.data);
    })
    .catch(error=>{
      console.log(error);
    })
  }

  //Insere lançamento
  const lancamentoPost = async () => {
    try {
      const response = await Api.post("lancamentos", {
        ...lancamentoSelecionado,
        id_Usuario: parseInt(lancamentoSelecionado.id_Usuario),
        natureza: parseInt(lancamentoSelecionado.natureza),
        tipo: parseInt(lancamentoSelecionado.tipo),
        descricao: lancamentoSelecionado.descricao,
        valor: parseFloat(lancamentoSelecionado.valor),
        data_Pagamento: lancamentoSelecionado.data_Pagamento ? format(new Date(lancamentoSelecionado.data_Pagamento), "yyyy-MM-dd'T'HH:mm:ss") : null,
        data_Cadastro: lancamentoSelecionado.data_Cadastro ? format(new Date(lancamentoSelecionado.data_Cadastro), "yyyy-MM-dd'T'HH:mm:ss") : null,
        observacao: lancamentoSelecionado.observacao,
      });
  
      setData(data.concat(response.data));
      showHideInsert();
      await lancamentoGet();
    } catch (error) {
      console.log(error);
    }
  };
  
  //Altera lançamento
  const lancamentoPut = async () => {
    try {
      const response = await Api.put(`lancamentos/${lancamentoSelecionado.id}`, {
        id: lancamentoSelecionado.id,
        id_Usuario: parseInt(lancamentoSelecionado.id_Usuario),
        natureza: parseInt(lancamentoSelecionado.natureza),
        tipo: parseInt(lancamentoSelecionado.tipo),
        descricao: lancamentoSelecionado.descricao,
        valor: parseFloat(lancamentoSelecionado.valor),
        data_Pagamento: lancamentoSelecionado.data_Pagamento ? format(new Date(lancamentoSelecionado.data_Pagamento), "yyyy-MM-dd'T'HH:mm:ss") : null,
        data_Cadastro: lancamentoSelecionado.data_Cadastro ? format(new Date(lancamentoSelecionado.data_Cadastro), "yyyy-MM-dd'T'HH:mm:ss") : null,
        observacao: lancamentoSelecionado.observacao,
      });
  
      if (response.status === 200) {
        setData(data.map(lancamento =>
          lancamento.id === lancamentoSelecionado.id ? response.data : lancamento
        ));
        showHideEdit();
        await lancamentoGet();
      }
    } catch (error) {
      console.log(error);
    }
  }
 
  //Exclui lançamento
  const lancamentoDelete = async () => {
    try {
      const response = await Api.delete(`lancamentos/${lancamentoSelecionado.id}`);
      if (response.status === 200) {
        setData(data.filter(lancamento => lancamento.id !== lancamentoSelecionado.id));
        showHideDelete();
        console.log(lancamentoSelecionado.id);
      } else {
        console.log("Falha ao excluir lançamento");
      }
      await lancamentoGet();
    } catch (error) {
      console.log(error);
    }
  }
  
  const selecionarLancamento = (usuario: SetStateAction<ILancamento>, caso: string) => {
    setLancamentoSelecionado(usuario);
      (caso === "Editar") &&
        showHideEdit();
      (caso === "Excluir") &&
        showHideDelete();
  }

  const getTipoLabel = (tipo: number) => {
    return tipo === 1 ? 'Crédito' : tipo === 2 ? 'Débito' : '';
  };

  const getNaturezaLabel = (tipo: number) => {
    return tipo === 1 ? 'Despesa' : tipo === 2 ? 'Receita' : '';
  };
  
  const handleChangeDate = (date: Date | null) => {
    if (date && !isNaN(date.getTime())) {
      setLancamentoSelecionado({
        ...lancamentoSelecionado,
        data_Pagamento: date,
      });
    }
  };
  
  useEffect(()=>{
    const fetchData = async () => {
      await lancamentoGet();
    }
    fetchData();
  }, [])

  return (
    <>
      <Navmenu />
      <div className="container">
        <h3>Lançamentos</h3>
        <Row className="justify-content-left">
          <Col xs="auto" className='d-flex align-items-end justify-content-end my-3'>
            <Button className="btn btn-success" onClick={showHideInsert}>
              {'Cadastrar'}
            </Button>
          </Col>
        </Row>
        <Table striped bordered hover>
          <thead>
            <tr>
              <th>Data Pgto</th>
              <th>Natureza</th>
              <th>Descricao</th>
              <th>Tipo</th>
              <th>Valor</th>
              <th>Obs</th>
            </tr>
          </thead>
          <tbody>
              {currentData.length > 0 ? currentData.map(lancamento => (
                  <tr key={lancamento.id}>
                    <td>{lancamento.data_Pagamento ? format(new Date(lancamento.data_Pagamento), 'dd/MM/yyyy') : ''}</td>
                    <td>{getNaturezaLabel(lancamento.natureza)}</td>
                    <td>{lancamento.descricao}</td>
                    <td>{getTipoLabel(lancamento.tipo)}</td>
                    <td className={lancamento.tipo === 2 ? 'text-danger' : 'text-success'}>
                      {lancamento.valor !== undefined
                        ? `${lancamento.tipo === 2 ? '-' : ''}R$ ${lancamento.valor.toFixed(2)}`
                        : ''}
                    </td>
                    <td>{lancamento.observacao}</td>
                    <td>
                      <button className="btn btn-primary" onClick={() => selecionarLancamento(lancamento, "Editar")}>Editar</button>{"  "}
                      <button className="btn btn-secondary" onClick={() => selecionarLancamento(lancamento, "Excluir")}>Excluir</button>
                    </td>
                  </tr>
                )): <tr><td colSpan={10}>Nenhum resultado encontrado.</td></tr>
              }
          </tbody>
        </Table>     
        <Pagination
          dataPerPage={dataPerPage}
          totalData={data.length}
          paginate={paginate}
        />
        <Modal isOpen={showInsert}>
          <ModalHeader>Cadastrar Lançamento</ModalHeader>
          <ModalBody>
            <div className="form-group">
              <label>Data Pgto: </label>
              <br />
              <DateTimePicker selectedData={lancamentoSelecionado.data_Pagamento ? new Date(lancamentoSelecionado.data_Pagamento) : new Date()} handlerData={handleChangeDate} />
              <br />
              <label>Natureza: </label>
              <br />
              <SelectNatureza status={lancamentoSelecionado.natureza} handlerStatus={(value) => handleChange({ target: { name: 'natureza', value } })} />
              <br />
              <label>Descrição: </label>
              <br />
              <input type="text" className="form-control" name="descricao"  onChange={handleChange}/>
              <br />
              <label>Tipo: </label>
              <br />
              <SelectTipo status={lancamentoSelecionado.tipo} handlerStatus={(value) => handleChange({ target: { name: 'tipo', value } })} />
              <br />
              <label>Valor: </label>
              <br />
              <input type="number" className="form-control" name="valor"  onChange={handleChange}/>
              <br />
              <label>Obs: </label>
              <br />
              <input type="text" className="form-control" name="observacao"  onChange={handleChange}/>
              <br />
            </div>
          </ModalBody>
          <ModalFooter>
            <button className="btn btn-primary" onClick={()=>lancamentoPost()}>Salvar</button>{"   "}
            <button className="btn btn-danger" onClick={()=>showHideInsert()}>Cancelar</button>
          </ModalFooter>
        </Modal>

        <Modal isOpen={showEdit}>
          <ModalHeader>Editar Lançamento</ModalHeader>
          <ModalBody>
            <div className="form-group">
              <input type="text" className="form-control" readOnly hidden value={lancamentoSelecionado && lancamentoSelecionado.id}/>       
              <label>Data Pgto: </label>
              <br />
              <DateTimePicker selectedData={lancamentoSelecionado.data_Pagamento ? new Date(lancamentoSelecionado.data_Pagamento) : new Date()} handlerData={handleChangeDate} />
              <br />
              <label>Natureza: </label>
              <br />
              <SelectNatureza status={lancamentoSelecionado.natureza} handlerStatus={(value) => handleChange({ target: { name: 'natureza', value } })}/>
              <br />
              <label>Descrição: </label>
              <br />
              <input type="text" className="form-control" name="descricao" onChange={handleChange} value={lancamentoSelecionado && lancamentoSelecionado.descricao}/>
              <br />
              <label>Tipo: </label>
              <br />
              <SelectTipo status={lancamentoSelecionado.tipo} handlerStatus={(value) => handleChange({ target: { name: 'tipo', value } })}/>
              <br />
              <label>Valor: </label>
              <br />
              <input type="number" className="form-control" name="valor" onChange={handleChange} value={lancamentoSelecionado && lancamentoSelecionado.valor !== undefined ? lancamentoSelecionado.valor : ''}/>
              <br />
              <label>Obs: </label>
              <br />
              <input type="text" className="form-control" name="obs" onChange={handleChange} value={lancamentoSelecionado && lancamentoSelecionado.observacao}/>
              <br />
            </div>
          </ModalBody>
          <ModalFooter>
            <button className="btn btn-primary" onClick={()=>lancamentoPut()}>Salvar</button>{"   "}
            <button className="btn btn-danger" onClick={()=>showHideEdit()}>Cancelar</button>
          </ModalFooter>
        </Modal>

        <Modal isOpen={showDelete}>
          <ModalBody>
            Confirma a exclusão do lançamento: {lancamentoSelecionado && lancamentoSelecionado.descricao} ?
          </ModalBody>
          <ModalFooter>
            <button className="btn btn-danger" onClick={()=>lancamentoDelete()}>
              Sim
            </button>
            <button className="btn btn-secondary" onClick={()=>showHideDelete()}>
              Não
            </button>
          </ModalFooter>
        </Modal>
      </div>
    </>
  );
}
