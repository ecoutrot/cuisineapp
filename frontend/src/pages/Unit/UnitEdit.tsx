import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { fetchUnitApi } from "../../services/Unit";
import { Unit } from "../../types/Unit";
import UnitForm from "../../components/Unit/UnitForm";
import Spinner from "../../components/Elements/Spinner";

function UnitEdit() {
  const { id } = useParams();

  const [unitToEdit, setUnitToEdit] = useState<Unit>();

  useEffect(() => {
    if (id) {
      fetchUnitApi(id)
        .then((recipe) => {
          if (!recipe) return;
          setUnitToEdit(recipe);
        })
        .catch((error: unknown) => {
          console.error(error);
        });
    }
  }, [id]);

  if (!unitToEdit) {
    return <Spinner />;
  }

  return (
    <>
      <UnitForm unit={unitToEdit} />
    </>
  );
}

export default UnitEdit;
