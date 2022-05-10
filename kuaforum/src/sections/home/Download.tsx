// @mui material components
import { Box, Typography, Button } from "@mui/material";
import Container from "@mui/material/Container";
import Grid from "@mui/material/Grid";
import Tooltip from "@mui/material/Tooltip";

// Images
import bgImage from "../../assets/images/waves-white.svg";

function Download() {
  return (
    <Box component="section" py={{ xs: 0, sm: 12 }}>
      <Box
        sx={{ margin:'auto', maxWidth:'90%', backgroundImage:`url(${bgImage})`, overflow: "hidden", borderRadius:"xl",position:"relative" ,bgColor:"dark", variant:"gradient" }}
      >
        <Container sx={{ position: "relative", zIndex: 2, py: 12 }}>
          <Grid container item xs={12} md={7} justifyContent="center" mx="auto" textAlign="center">
            <Typography variant="h3" color="white">
              Do you love this awesome
            </Typography>
            <Typography variant="h3" color="white" mb={1}>
              UI Kit for ReactJS &amp; MUI?
            </Typography>
            <Typography variant="body2" color="white" mb={6}>
              Cause if you do, it can be yours for FREE. Hit the button below to navigate to
              Creative Tim where you can find the Design System in HTML. Start a new project or give
              an old Bootstrap project a new look!
            </Typography>
            <Button
              variant="contained"
              color="info"
              size="large"
              component="a"
              href="https://www.creative-tim.com/product/material-kit-react"
              sx={{ mb: 2 }}
            >
              Kuaförünü Bul
            </Button>
          </Grid>
        </Container>
      </Box>

    </Box>
  );
}

export default Download;
