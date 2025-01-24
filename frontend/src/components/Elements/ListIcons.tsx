
function ListIcons({index, icon}: {index: number, icon: string}) {

    const length = index + 1

    return (
        <>
            {Array.from({ length: length }, (_, i) => (
                <img
                    key={i}
                    src={icon}
                    alt=""
                    className="inline-flex size-6"
                />
            ))}
        </>
    )
}

export default ListIcons