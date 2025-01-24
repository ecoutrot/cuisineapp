import { useEffect, useState } from "react";
import { Unit } from "../../types/Unit";
import UnitListCardItem from "../../components/Unit/UnitListCardItem";
import { nanoid } from "nanoid";
import { fetchUnitsApi } from "../../services/Unit";
import { FaChevronLeft, FaChevronRight } from "react-icons/fa";
import Spinner from "../../components/Elements/Spinner";

function UnitsView() {
  const limit = 10;

  const [units, setUnits] = useState<Unit[]>();
  const [page, setPage] = useState<number>(1);

  const handleIncrementPage = (page: number) => {
    if (units?.length === limit) {
      setPage(page + 1);
    }
  };
  const handleDecrementPage = (page: number) => {
    if (page > 1) {
      setPage(page - 1);
    }
  };

  const handleDelete = (id: string) => {
    setUnits(units?.filter((unit) => unit.id !== id));
  };

  useEffect(() => {
    fetchUnitsApi(page, limit)
      .then((units) => {
        if (!units) return;
        setUnits(units);
      })
      .catch((error: unknown) => {
        console.error(error);
      });
  }, [page, limit]);

  if (!units) {
    return <Spinner />;
  }

  const listeUnits = units.map((unit: Unit) => {
    return <UnitListCardItem key={nanoid()} unit={unit} onDelete={handleDelete} />;
  });

  return (
    <>
      <div className="relative mx-auto mb-6 max-w-3xl rounded-lg border-t bg-white p-6 shadow-md">
        {listeUnits.length > 0 && (
          <>
            <div className="relative mt-6 flex flex-col">
              <nav className="flex min-w-[240px] flex-col gap-1">{listeUnits}</nav>
            </div>
            <ol className="my-6 flex justify-center gap-2 text-lg font-medium">
              <li>
                <button
                  onClick={() => handleDecrementPage(page)}
                  className={`inline-flex size-10 items-center justify-center rounded-full border border-gray-200 bg-white ${
                    page > 1 ? "text-gray-900 hover:bg-gray-100" : "cursor-not-allowed text-gray-300"
                  }`}
                  disabled={page <= 1}
                >
                  <FaChevronLeft />
                </button>
              </li>

              <li className="inline-flex size-10 items-center justify-center rounded-full border border-indigo-600 bg-indigo-600 text-white">{page}</li>

              <li>
                <button
                  onClick={() => handleIncrementPage(page)}
                  className={`inline-flex size-10 items-center justify-center rounded-full border border-gray-200 bg-white ${
                    units?.length === limit ? "text-gray-900 hover:bg-gray-100" : "cursor-not-allowed text-gray-300"
                  }`}
                  disabled={units?.length !== limit}
                >
                  <FaChevronRight />
                </button>
              </li>
            </ol>
          </>
        )}
      </div>
    </>
  );
}

export default UnitsView;
