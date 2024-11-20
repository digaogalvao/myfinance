import Form from 'react-bootstrap/Form';

const optionMonths = [
  {
    label: "Despesa",
    value: "1",
  },
  {
    label: "Receita",
    value: "2",
  },
];

type Props = {
  status: string,
  handlerStatus: (status: string) => void
};

export const SelectNatureza = ({ status, handlerStatus}: Props) => {

  const selectNatureza = (status: String) => {
    console.log("selectNatureza = " + status);
    handlerStatus(status.toString());
  };

  return (
    <Form.Select id="selectNatureza" value={status} onChange={e => selectNatureza(e.target.value)}>
      <option defaultChecked>Selecione</option>
      {optionMonths.map((item) => (
        <option value={item.value}>{item.label}</option>
      ))}
    </Form.Select>
  );
};
