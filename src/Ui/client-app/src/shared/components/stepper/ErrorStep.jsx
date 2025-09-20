export function ErrorStep({ errorSteps }) {
  //test click
  const getclick = () => console.log("click");
  return (
    <>
      {errorSteps.length > 0 && (
        <div className="error-section">
          <hr className="my-3" />
          <div
            className="d-flex align-items-center mb-2 pt-2"
            style={{ fontSize: "15px", fontWeight: "400" }}
          >
            <div className="text-danger">
              <i className="bi bi-exclamation-triangle-fill me-2"></i>
            </div>

            <div className="d-flex flex-column gap-2">
              <div
                style={{
                  fontSize: "13.125px",
                  fontFamily: '"Inter',
                  fontWeight: 600,
                }}
              >
                Partially completed
              </div>
            </div>
            <button
              className="m-auto border px-3"
              style={{
                fontSize: "13.125px",
                fontFamily: '"Inter',
                fontWeight: 500,
                borderRadius: "4px",
              }}
              onClick={() => getclick()}
            >
              Retry
            </button>
          </div>
          {errorSteps.map((step) => (
            <div
              key={`error-${step.id}`}
              className=" py-0 mb-2"
              style={{ fontSize: "11px", fontWeight: 400, color: "grey" }}
              role="alert"
            >
              <span>Last updated: 2025/9/18, 16:33</span>
              <br />
              <span>{step.error}</span> <a href="#">Learn why</a>
            </div>
          ))}
        </div>
      )}
    </>
  );
}
