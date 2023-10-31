import styles from '../styles/Text.module.scss'
function Text({ label, children }: TextProps) {
    return (
        <div className={styles.container}>
            <label>{label}</label>
            <span className={styles['text']}>{children}</span>
        </div>
    )
}

export default Text
type TextProps = {
    label: string,
    children: React.ReactNode,
}