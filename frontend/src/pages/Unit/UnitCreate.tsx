
import { Unit } from "../../types/Unit";
import UnitForm from "../../components/Unit/UnitForm";


function UnitCreate() {

  const newUnit: Unit | null = null;

  return (
    <>
      <UnitForm unit={newUnit} />
    </>
  );
}

export default UnitCreate;
