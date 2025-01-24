import { Link } from "react-router-dom";
import { Unit } from "../../types/Unit";
import { FaPencilAlt } from "react-icons/fa";
import { useState } from "react";
import { deleteUnitApi } from "../../services/Unit";
import { FaRegTrashCan } from "react-icons/fa6";

function UnitListCardItem({ unit, onDelete }: { unit: Unit; onDelete: (id: string) => void }) {
  const [isDeleting, setIsDeleting] = useState(false);

  const handleDelete = async () => {
    if (!window.confirm(`Êtes-vous sûr de vouloir supprimer "${unit.name}" ?`)) {
      return;
    }
    setIsDeleting(true);
    if (!unit.id) {
      setIsDeleting(false);
      return;
    }
    try {
      await deleteUnitApi(unit.id);
      onDelete(unit.id);
    } catch (error) {
      console.error("Error deleting unit:", error);
    } finally {
      setIsDeleting(false);
    }
  };
  return (
    <div className="flex w-full items-center rounded-md border border-gray-200 p-4 shadow-sm transition-all hover:bg-gray-50">
      <div className="grow">
        <div className="flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between">
          <div>
            <h3 className="mt-2 text-lg font-bold text-gray-800">{unit.name}</h3>
          </div>
        </div>
      </div>
      <div className="flex gap-3">
        <Link
          to={`/units/edit/${unit.id}`}
          className="flex size-10 items-center justify-center rounded-full bg-gray-100 text-indigo-600 hover:bg-indigo-200 focus:outline-none focus:ring focus:ring-indigo-300"
          title="Éditer l'ingrédient"
        >
          <FaPencilAlt />
        </Link>
        <button
          className="flex size-10 items-center justify-center rounded-full bg-gray-100 text-red-600 hover:bg-red-600 hover:text-white focus:outline-none focus:ring"
          type="button"
          onClick={() => void handleDelete()}
          disabled={isDeleting}
        >
          <FaRegTrashCan />
        </button>
      </div>
    </div>
  );
}

export default UnitListCardItem;
