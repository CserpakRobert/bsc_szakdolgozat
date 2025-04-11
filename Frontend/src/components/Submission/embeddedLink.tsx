
export default function EmbeddedLink({ link, title }: { link: string, title: string }) {

    return (
        <>
            <iframe
                src={`https://www.youtube.com/embed/${link}`}
                allow='autoplay; encrypted-media'
                title={title}
                width={"100%"}
                height={"100%"}
            />

        </>
    )
}