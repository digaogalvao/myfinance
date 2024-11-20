import Form from 'react-bootstrap/Form';

const optionMonths = [
  {
    label: "Crédito",
    value: "1",
  },
  {
    label: "Débito",
    value: "2",
  },
];

type Props = {
  status: string,
  handlerStatus: (status: string) => void
};

export const SelectTipo = ({ status, handlerStatus}: Props) => {

  const selectTipo = (status: String) => {
    console.log("selectTipo = " + status);
    handlerStatus(status.toString());
  };

  return (
    <Form.Select id="selectTipo" value={status} onChange={e => selectTipo(e.target.value)}>
      <option defaultChecked>Selecione</option>
      {optionMonths.map((item) => (
        <option value={item.value}>{item.label}</option>
      ))}
    </Form.Select>
  );
};
