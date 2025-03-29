
export default function EmbeddedLink({ link, title }: { link: string, title: string }) {

    return (
        <>
            <iframe
                src={link}
                allow='autoplay; encrypted-media'
                title={title}
            />

        </>
    )
}